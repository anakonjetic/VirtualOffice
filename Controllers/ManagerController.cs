using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Diagnostics;
using System.Text;
using VirtualOffice.Data;
using VirtualOffice.Models;

namespace VirtualOffice.Controllers
{
    public class ManagerController : Controller
    {

        private ApplicationDbContext _dbContext;
        private UserManager<IdentityUser> _userManager;
        private DateTime? clockInTime; // Variable to store clock in time

        public ManagerController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Or LicenseContext.Commercial for commercial use

        }

        public IActionResult ManagerHomePage()
        {
            return View();
        }

        //handlea loadanje Partial View objekata unutar content sekcije u ManagerHomePage
        public IActionResult LoadPartialView(string target)
        {
            //dohvaćanje podataka za model poslan u partial view --start
            string loggedInUserId = User.Identity.Name;

            Employee loggedInEmployee = this._dbContext.Employee.FirstOrDefault(e => e.UserId == loggedInUserId);

            if (loggedInEmployee == null)
            {
                return NotFound();
            }

            var employeeModel = GetEmployeeManagementModel(loggedInUserId);

            var managedTeamIds = GetTeamManagementModel(loggedInUserId);

            var teamModel = GetAllTeamData();

            var employees = GetAllEmployees();

            var equipmentNamesDictionary = GetAllEquipmentNames(employees);

            var clockIns = getClockIns();

            var teamManagers = GetManagersByTeam(null);        


            var teamManagementModel = new TeamManagementWrapperModel
            {
                TeamList = teamModel,
                IntList = managedTeamIds,
                ManagerNames = teamManagers
            };

            var dataExport = new EmployeeViewModel
            {
                Employees = employees,
                EquipmentNamesDictionary = equipmentNamesDictionary
            };

            var clockIn = new ClockInViewModel
            {
                Employees = employees,
                ClockIns = clockIns

            };

            var requestModel = setRequestTableModel();

            var equipmentModel = setEquipmentForManagerTabModel();

            //dohvaćanje podataka za model poslan u partial view --end

            //dohvaćanje Partial View objekata ovisno o odabranom Nav Itemu
            switch (target)
            {
                case "home":
                    return PartialView("_ManagerHome");
                case "employee":
                    return PartialView("_ManagerEmployeeTable", employeeModel); //napravljen samo popis zaposlenika iz timova koji su predvođeni logged in userom
                case "evaluation":
                    return PartialView("_ManagerEvaluation");
                case "office":
                    return PartialView("_ManagerOutOfOffice", requestModel);
                case "equipment":
                    return PartialView("_ManagerEquipmentManagement", equipmentModel);
                case "clock":
                    return PartialView("_ManagerClockIn", clockIn);
                case "hierarchy":
                    return PartialView("_ManagerHierarchy");
                case "team":
                    return PartialView("_ManagerTeamTable", teamManagementModel);
                case "export":
                    return PartialView("_ManagerDataExport", dataExport);
                case "list":
                    return PartialView("_EmployeeList", dataExport);
                case "create":
                    return PartialView("_CreateEmployee");

                default:
                    return PartialView("_ManagerHome");
            }
        }

        public IActionResult TeamSummary(int teamId)
        {
            var team = _dbContext.Team.FirstOrDefault(t => t.Id == teamId);
            var employees = _dbContext.Employee
                            .Where(e => e.TeamId == teamId)
                            .ToList();

            var teamDetailsWrapper = new TeamDetailsWrapperModel
            {
                Team = team,
                Employees = employees
            };

            return PartialView("_TeamSummary", teamDetailsWrapper);
        }


        [HttpPost]
        public ActionResult ClockIn()
        {
            string userId = User.Identity.Name;

            // Check if the user has already clocked in for today
            bool hasClockedIn = _dbContext.ClockIns
                                        .Any(c => c.Employee.UserId == userId && c.ClockInTime.Date == DateTime.Today);

            if (hasClockedIn)
            {
                return Json(new { success = false, message = "Already clocked in for today!" });
            }

            // Record clock in time in the database
            ClockIn clockIn = new ClockIn
            {
                EmployeeId = _dbContext.Employee.FirstOrDefault(e => e.UserId == userId)?.Id ?? 0,
                ClockInTime = DateTime.Now,
                Date = DateTime.Today
            };

            _dbContext.ClockIns.Add(clockIn);
            _dbContext.SaveChanges();

            return Json(new { success = true });
        }

        // POST: /Manager/ClockOut
        [HttpPost]
        public ActionResult ClockOut()
        {
            string userId = User.Identity.Name;

            // Get the user's last clock-in record for today
            ClockIn lastClockIn = _dbContext.ClockIns
                                            .Where(c => c.Employee.UserId == userId && c.ClockInTime.Date == DateTime.Today)
                                            .OrderByDescending(c => c.ClockInTime)
                                            .FirstOrDefault();

            //Console.WriteLine("test");

            if (lastClockIn == null)
            {
                return Json(new { success = false, message = "You haven't clocked in yet!" });
            }

            if (lastClockIn.ClockOutTime != DateTime.MinValue)
            {
                return Json(new { success = false, message = "You've already clocked out!" });
            }


            // Record clock out time in the database
            lastClockIn.ClockOutTime = DateTime.Now;
            _dbContext.SaveChanges();

            TimeSpan timeWorked = lastClockIn.ClockOutTime - lastClockIn.ClockInTime;

            return Json(new { success = true, timeWorked = timeWorked.ToString(@"hh\:mm\:ss") });
        }


        public IActionResult ExportToExcel()
        {
            var employees = this._dbContext.Employee.ToList();
            employees = this._dbContext.Employee.Include(e => e.Team).ToList();


            using (var excelPackage = new ExcelPackage())
            {
                var worksheet = excelPackage.Workbook.Worksheets.Add("Employees");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "First Name";
                worksheet.Cells[1, 3].Value = "Last Name";
                worksheet.Cells[1, 4].Value = "Date of Birth";
                worksheet.Cells[1, 5].Value = "Remaining Days Off";
                worksheet.Cells[1, 6].Value = "Sick Leave Days Used";
                worksheet.Cells[1, 7].Value = "Equipment";
                worksheet.Cells[1, 8].Value = "Team";
                worksheet.Cells[1, 9].Value = "User ID";

                // Add data
                int row = 2;
                foreach (var employee in employees)
                {
                    var equipmentIds = employee.EquipmentId.Split('#').Where(id => !string.IsNullOrEmpty(id));

                    var equipmentNames = new List<string>();

                    foreach (var equipmentId in equipmentIds)
                    {
                        if (int.TryParse(equipmentId, out int id))
                        {
                            var equipment = this._dbContext.Equipment.FirstOrDefault(e => e.Id == id);
                            if (equipment != null)
                            {
                                equipmentNames.Add(equipment.Name);
                            }
                        }
                    }

                    worksheet.Cells[row, 1].Value = employee.Id;
                    worksheet.Cells[row, 2].Value = employee.FirstName;
                    worksheet.Cells[row, 3].Value = employee.LastName;
                    worksheet.Cells[row, 4].Value = employee.DateOfBirth.ToShortDateString();
                    worksheet.Cells[row, 5].Value = employee.RemainingDaysOff;
                    worksheet.Cells[row, 6].Value = employee.SickLeaveDaysUsed;
                    worksheet.Cells[row, 7].Value = string.Join(", ", equipmentNames); // Join equipment names into a single string
                    worksheet.Cells[row, 8].Value = employee.Team != null ? employee.Team.Name : "N/A";
                    worksheet.Cells[row, 9].Value = employee.UserId != null ? employee.UserId : "N/A";
                    row++;
                }

                // Convert the Excel package to a byte array
                byte[] excelBytes = excelPackage.GetAsByteArray();

                // Return the Excel file
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Employees.xlsx");
            }
        }


        public IActionResult EditTeam(int teamId)
        {

            // Fetch available managers and employees from the database
            var managers = _dbContext.Employee.ToList();
            var allEmployees = _dbContext.Employee.ToList();
            List<Employee> teamEmployees = new List<Employee>();
            var team = new Team();

            Employee selectedManager = null;

            if (teamId != null)
            {
                // Fetch the team to be edited
                team = _dbContext.Team
                    .Include(t => t.Employee) // Include employees associated with the team
                    .FirstOrDefault(t => t.Id == teamId);

                if (team != null)
                {
                    var teamManagers = _dbContext.Employee
                    .Where(e => e.TeamId == team.Id && _dbContext.EmployeeManager.Any(em => em.ManagerId == e.Id))
                    .Select(e => $"{e.FirstName} {e.LastName}")
                    .ToList();

                    var managerInfo = _dbContext.Employee
                        .Where(e => e.TeamId == team.Id && _dbContext.EmployeeManager.Any(em => em.ManagerId == e.Id))
                        .Select(e => e)
                        .FirstOrDefault();


                    teamEmployees = allEmployees.Where(e => e.TeamId == team.Id).ToList();

                    selectedManager = managerInfo;
                }
            }

            // Pass managers and employees to the view
            ViewData["SelectedManager"] = selectedManager;
            ViewData["Managers"] = managers;
            ViewData["Employees"] = teamEmployees;

            var teamViewModel = new TeamViewModel { 
                Id = team.Id,
                Name = team.Name,
                ManagerId = _dbContext.Employee
                        .Where(e => e.TeamId == team.Id && _dbContext.EmployeeManager.Any(em => em.ManagerId == e.Id))
                        .Select(e => e.Id)
                        .FirstOrDefault(),
                Managers = managers,
                SelectedEmployeeIds = teamEmployees.Select(e => e.Id).ToList(),
                AvailableEmployees = allEmployees
                };

            return PartialView("_TeamCreate", teamViewModel);
        }

        public IActionResult TeamCreate()
        {
            List<int> employeeIdsWithManagers = _dbContext.EmployeeManager
                .Select(em => em.ManagerId)
                .ToList();

            var managers = _dbContext.Employee.ToList();
            var employees = _dbContext.Employee
                .Where(e => !employeeIdsWithManagers.Contains(e.Id))
                .ToList();


            ViewData["Managers"] = managers;
            ViewData["Employees"] = employees;

            return PartialView("_TeamCreate", null);
        }

        public IActionResult SaveTeam(TeamViewModel model)
        {
            Team team;

            if (model.Id == 0) {
                team = new Team();

                team.Name = model.Name;

                var selectedManagerId = model.ManagerId;

                var selectedManager = _dbContext.Employee
                    .Where(em => em.Id == selectedManagerId)
                    .SingleOrDefault();

                List<Employee> teamEmployees = _dbContext.Employee
                       .Where(e => model.SelectedEmployeeIds.Contains(e.Id))
                       .ToList();

               

                var employeeManager = new EmployeeManager
                {
                    ManagerId = selectedManagerId,
                    EmployeeId = selectedManagerId
                };

                _dbContext.Team.Add(team);
                _dbContext.EmployeeManager.Add(employeeManager);

                _dbContext.EmployeeManager.Add(employeeManager);

                _dbContext.SaveChanges();

                selectedManager.TeamId = team.Id;

                foreach (var teamEmployee in teamEmployees)
                {
                    teamEmployee.TeamId = team.Id;
                }

                var employeesToRemove = _dbContext.EmployeeManager
                           .Where(em => em.EmployeeId == selectedManagerId && _dbContext.Employee.Any(e => e.TeamId == team.Id))
                           .ToList();

                _dbContext.EmployeeManager.RemoveRange(employeesToRemove);               

                _dbContext.SaveChanges();

            }
            else
            {
                team = _dbContext.Team
                       .Include(t => t.Employee)
                       .FirstOrDefault(t => t.Id == model.Id);

                
                team.Name = model.Name;

                var selectedManagerId = model.ManagerId;

                var previousManagerId = _dbContext.Employee
                        .Where(e => e.TeamId == team.Id && _dbContext.EmployeeManager.Any(em => em.ManagerId == e.Id))
                        .Select(e => e.Id)
                        .SingleOrDefault();

                if (selectedManagerId != 0)
                {
                    var teamEmployees = _dbContext.Employee.Where(e => e.TeamId == team.Id).ToList(); 
                    var selectedEmployees = _dbContext.Employee
                           .Where(e => model.SelectedEmployeeIds.Contains(e.Id))
                           .ToList();

                    foreach (var teamEmployee in teamEmployees)
                    {
                        if (!selectedEmployees.Select(e => e.Id).ToList().Contains(teamEmployee.Id))
                        {
                            
                            teamEmployee.TeamId = 1;
                        }
                    }

                    foreach (var selected in selectedEmployees)
                    {
                        if (!teamEmployees.Select(e => e.Id).ToList().Contains(selected.Id))
                        {

                            selected.TeamId = model.Id;
                        }
                    }



                    if (previousManagerId != null && previousManagerId != selectedManagerId)
                    {

                        var employeesToRemove = _dbContext.EmployeeManager
                            .Where(em => em.ManagerId == previousManagerId && _dbContext.Employee.Any(e => e.Id == em.EmployeeId && e.TeamId == team.Id))
                            .ToList();

                        _dbContext.EmployeeManager.RemoveRange(employeesToRemove);

                        _dbContext.SaveChanges();

                        teamEmployees = _dbContext.Employee.Where(e => e.TeamId == team.Id).ToList();

                        var employeeManagers = new List<EmployeeManager>();

                        foreach (var employee in teamEmployees)
                        {
                            if (employee.Id != selectedManagerId)
                            {
                                var employeeEmployeeManager = new EmployeeManager
                                {
                                    ManagerId = selectedManagerId,
                                    EmployeeId = employee.Id
                                };

                                employeeManagers.Add(employeeEmployeeManager);
                            }
                        }

                        if (employeeManagers != null)
                        {
                            foreach (var managerRow in employeeManagers)
                            {
                                _dbContext.EmployeeManager.Add(managerRow);
                            }
                        }

                        
                    }

                    


                    _dbContext.SaveChanges();
                }
            }

            

            return PartialView("ManagerHomePage", "team");
        }

        public IActionResult TeamDelete(int teamId)
        {
            string loggedInUserId = User.Identity.Name;
            var team = _dbContext.Team.FirstOrDefault(t => t.Id == teamId);
           

            var employeesToReassign = _dbContext.Employee.Where(e => e.TeamId == teamId).ToList();

            var team1Id = 1; 
            foreach (var employee in employeesToReassign)
            {
                employee.TeamId = team1Id;
            }
            _dbContext.SaveChanges();

            _dbContext.Remove(team);

            var managementToDelete = _dbContext.EmployeeManager
                .Where(e => e.ManagerId == e.EmployeeId && _dbContext.Employee.Any(em=> employeesToReassign.Select(res => res.Id).Contains( em.Id) && em.Id == e.ManagerId))
                .ToList();

            _dbContext.RemoveRange(managementToDelete);

            _dbContext.SaveChanges();

            var teamModel = GetAllTeamData();

            var employees = GetAllEmployees();

            var teamManagers = GetManagersByTeam(null);

            var managedTeamIds = GetTeamManagementModel(loggedInUserId);
            
            var teamManagementModel = new TeamManagementWrapperModel
            {
                TeamList = teamModel,
                IntList = managedTeamIds,
                ManagerNames = teamManagers
            };


            return PartialView("_ManagerTeamTable", teamManagementModel);
        }

        public IActionResult RequestDecision(RequestOoOManagerViewModel model)
        {
            var request = _dbContext.Request.Where(r => r.Id == model.RequestID).Include(r => r.Status).FirstOrDefault();
            var employee = _dbContext.Employee.Where(e => e.Id == model.EmployeeID).FirstOrDefault();

            if(request.RequestTypeID == 1)
            {
                if ((bool)model.IsApproved)
                {
                    request.StatusId = 3;
                }
                else
                {
                    request.StatusId = 4;
                    request.Comment = model.Comment;
                    employee.RemainingDaysOff = model.RemainingDays;
                }
            } else if (request.RequestTypeID == 2)
            {
                employee.SickLeaveDaysUsed += model.Quantity;
                request.StatusId = 3;
            }

            _dbContext.SaveChanges();

            return View("ManagerHomePage", "office");

        }

        public IActionResult EquipmentRequestDecision(RequestEquipmentManagerViewModel model)
        {
            var request = _dbContext.Request.Where(r => r.Id == model.RequestID).Include(r => r.Status).FirstOrDefault();
            var employee = _dbContext.Employee.Where(e => e.Id == model.EmployeeID).FirstOrDefault();
            var requestedEquipment = _dbContext.Equipment
                .Where(e => e.Name == _dbContext.RequestType
                        .Where(r => r.Id == request.RequestTypeID)
                            .Select(r => r.Name).FirstOrDefault()).FirstOrDefault();
            

            if ((bool)model.IsApproved)
            {
                request.StatusId = 3;

                var assignedEquipmentIds = new List<int>();

                foreach (var id in employee.EquipmentId.Split('#'))
                {
                    assignedEquipmentIds.Add(int.Parse(id));
                };

                /*var assignedEquipment = _dbContext.Equipment.Where(e => assignedEquipmentIds.Contains(e.Id)).ToList();*/

                if (!assignedEquipmentIds.Contains(requestedEquipment.Id))
                {
                    assignedEquipmentIds.Add(requestedEquipment.Id);
                    employee.EquipmentId = String.Join('#', assignedEquipmentIds);
                }
            }
            else
            {
                request.StatusId = 4;
                request.Comment = model.Comment; 
            }

            _dbContext.SaveChanges();

            return View("ManagerHomePage", "equipment");

        }
        

        [HttpPost]
        public IActionResult Create(Employee model)
        {
            ModelState.Clear();
            model.SickLeaveDaysUsed = 0;
            model.RemainingDaysOff = 25;
            model.EquipmentId = string.Concat(model.EquipmentId, "1#4#6");

            string genEmail = string.Concat(model.FirstName.Substring(0, 1).ToLower(), model.LastName.ToLower());
            model.UserId = string.Concat(genEmail, "@tvz.hr");

            if (ModelState.IsValid)
            {
                this._dbContext.Employee.Add(model);
                this._dbContext.SaveChanges();

                var manager = new EmployeeManager();
                if (model.TeamId == 4)
                {
                    manager.ManagerId = 14;
                }
                else if (model.TeamId == 5)
                {
                    manager.ManagerId = 15;
                }
                else
                {
                    manager.ManagerId = model.TeamId;
                }
                manager.EmployeeId = model.Id;

                this._dbContext.EmployeeManager.Add(manager);
                this._dbContext.SaveChanges();

                return CreateNewUser(model).GetAwaiter().GetResult();

            }
            else
            {
                return View("ManagerHomePage", "employee");
            }
        }

        public async Task<IActionResult> CreateNewUser(Employee model)
        {
            var newUser = new IdentityUser
            {
                UserName = model.UserId,
                Email = model.UserId
            };

            string password = model.UserId.Substring(0, model.UserId.Length - 7);

            StringBuilder strB = new StringBuilder(password);

            strB[1] = char.ToUpper(password[1]);
            strB.Append("01@");

            var result = await _userManager.CreateAsync(newUser, strB.ToString());

            if (result.Succeeded)
            {
                return View("ManagerHomePage", "employee");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("ManagerHomePage", "team");
            }
        }

        public IActionResult Edit(int modelId)
        {
            var model = _dbContext.Employee.FirstOrDefault(c => c.Id == modelId);
            return PartialView("_EditEmployee", model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEmployee(int id)
        {
            var model = this._dbContext.Employee.Single(d => d.Id == id);
            var ok = await this.TryUpdateModelAsync(model);

            if (ok && this.ModelState.IsValid)
            {
                this._dbContext.SaveChanges();
                return View("ManagerHomePage", "employee");
            }

            return View("ManagerHomePage", "employee");
        }

        public IActionResult Delete(int id)
        {
            var employee = this._dbContext.Employee
                .Where(p => p.Id == id)
                .FirstOrDefault();

            if (employee == null)
            {
                return NotFound();
            }

            var employeeManager = this._dbContext.EmployeeManager
                .Where(p => p.EmployeeId == id);

            this._dbContext.EmployeeManager.RemoveRange(employeeManager);
            this._dbContext.Employee.Remove(employee);
            this._dbContext.SaveChanges();

            return View("ManagerHomePage", "employee");
        }


        //dohvaćanje svih zaposlenika koji se nalaze u timovima menadžiranih od strane logged in usera
        private List<Employee> GetEmployeeManagementModel(string loggedInUserId)
        {
            Employee loggedInEmployee = this._dbContext.Employee.FirstOrDefault(e => e.UserId == loggedInUserId);

            if (loggedInEmployee == null)
            {
                return null;
            }

            var managedTeamsQuery = this._dbContext.EmployeeManager
                .Where(em => em.ManagerId == loggedInEmployee.Id)
                .Select(em => em.Employee.TeamId);

            var employeesQuery = this._dbContext.Employee
                .Where(e => managedTeamsQuery.Contains(e.TeamId))
                .Include(e => e.Team);

            return employeesQuery.ToList();
        }


        private List<int> GetTeamManagementModel(string loggedInUserId)
        {
            Employee loggedInEmployee = this._dbContext.Employee.FirstOrDefault(e => e.UserId == loggedInUserId);

            if (loggedInEmployee == null)
            {
                return null;
            }

            var managedTeamsQuery = this._dbContext.EmployeeManager
                .Where(em => em.ManagerId == loggedInEmployee.Id)
                .Select(em => em.Employee.TeamId);



            return managedTeamsQuery.ToList();
        }

        private List<Team> GetAllTeamData()
        {

            var teamsQuery = this._dbContext.Team.AsQueryable();

            return teamsQuery.ToList();
        }

        private List<Employee> GetAllEmployees()
        {

            var employees = this._dbContext.Employee
             .FromSqlRaw("SELECT * FROM Employee");

            return employees.ToList();
        }


        public List<string> GetManagersByTeam(List<int> teamIds)
        {
            var managers = new List<string>();

            var teams = new List<Team>();
                
                if(teamIds != null)
            {
                teams = _dbContext.Team.Include(t => t.Employee)
                    .ToList()
                    .OrderBy(t => teamIds.IndexOf(t.Id))
                    .ToList();

            }
            else
            {
                teams = _dbContext.Team.Include(t => t.Employee)
                   .ToList();
            }

            foreach (var team in teams)
            {
                var teamManagers = _dbContext.Employee
                                            .Where(e => e.TeamId == team.Id && _dbContext.EmployeeManager.Any(em => em.ManagerId == e.Id))
                                            .Select(e => $"{e.FirstName} {e.LastName}")
                                            .ToList();

                if (teamManagers.Count == 0)
                {
                    var employees = _dbContext.EmployeeManager
                                                .Where(em => em.Employee.TeamId == team.Id && _dbContext.EmployeeManager.Any(e => e.EmployeeId == em.EmployeeId))
                                                .Select(em => em.EmployeeId)
                                                .ToList();

                    var correspondingManagers = _dbContext.EmployeeManager
                                                            .Where(em => employees.Contains(em.EmployeeId))
                                                            .Select(em => $"{em.Manager.FirstName} {em.Manager.LastName}")
                                                            .ToList();

                    teamManagers.AddRange(correspondingManagers);
                }

                managers.AddRange(teamManagers);
            }

            return managers;
        }

        private Dictionary<int, List<string>> GetAllEquipmentNames(List<Employee> employees)
        {
            var equipmentNamesDictionary = new Dictionary<int, List<string>>();

            foreach (var employee in employees)
            {
                var equipmentIds = employee.EquipmentId.Split('#').Where(id => !string.IsNullOrEmpty(id));

                var equipmentNames = new List<string>();

                foreach (var equipmentId in equipmentIds)
                {
                    if (int.TryParse(equipmentId, out int id))
                    {
                        var equipment = this._dbContext.Equipment.FirstOrDefault(e => e.Id == id);
                        if (equipment != null)
                        {
                            equipmentNames.Add(equipment.Name);
                        }
                    }
                }

                equipmentNamesDictionary.Add(employee.Id, equipmentNames);
            }

            return equipmentNamesDictionary;
        }

        private List<ClockIn> getClockIns()
        {
            return _dbContext.ClockIns.ToList();
        }

        public IActionResult FilterTeams(TeamFilterModel filter)
        {
            string loggedInUserId = User.Identity.Name;

            var managedTeamIds = GetTeamManagementModel(loggedInUserId);

            filter ??= new TeamFilterModel();
            var teamQuery = this._dbContext.Team.AsEnumerable();



            if (!string.IsNullOrWhiteSpace(filter.Name))
                teamQuery = teamQuery.Where(p => p.Name.ToLower().Contains(filter.Name.ToLower()));

            var model = teamQuery.ToList();

            var teamManagers = GetManagersByTeam(null);

            var teamManagementModel = new TeamManagementWrapperModel
            {
                TeamList = model,
                IntList = managedTeamIds,
                ManagerNames = teamManagers
            }; 

            return PartialView("_ManagerTeamTable", teamManagementModel);
        }

        public IActionResult SortTeams(string columnName, string sortDirection)
        {

            string loggedInUserId = User.Identity.Name;

            var managedTeamIds = GetTeamManagementModel(loggedInUserId);


            var teamsQuery = _dbContext.Team.AsQueryable();


            

            var testSortDir = sortDirection;

            // Apply sorting based on the columnName and sortDirection
            switch (columnName)
            {
                case "id":
                    teamsQuery = sortDirection == "asc" ? teamsQuery.OrderBy(t => t.Id) : teamsQuery.OrderByDescending(t => t.Id);
                    break;
                case "name":
                    teamsQuery = sortDirection == "asc" ? teamsQuery.OrderBy(t => t.Name) : teamsQuery.OrderByDescending(t => t.Name);
                    break;

                default:
                    break;
            }

            var teams = teamsQuery.ToList();

            var teamManagers = GetManagersByTeam(teams.Select(t => t.Id).ToList());

            var teamManagementModel = new TeamManagementWrapperModel
            {
                TeamList = teams,
                IntList = managedTeamIds,
                ManagerNames = teamManagers
            };

            return PartialView("_ManagerTeamTable", teamManagementModel);
        }

        public IActionResult RequestOoOManagerDetails(int requestId)
        {
            var request = _dbContext.Request.Where(r => r.Id == requestId.ToString()).FirstOrDefault();

            var employee = _dbContext.Employee.Where(e => e.Id == request.EmployeeId).FirstOrDefault();

            var remainingDaysAfterSubstraction = request.RequestTypeID == 1 ? employee.RemainingDaysOff - request.Quantity : 0;

            var approvable = remainingDaysAfterSubstraction >= 0;

            var isRequestClosed = request.StatusId == 3 || request.StatusId == 4 ? true : false;

            var requestModel = new RequestOoOManagerViewModel
            {
                RequestTypeID = request.RequestTypeID,
                RequestTypes = _dbContext.RequestType.Where(rt => rt.Id == 1 || rt.Id == 2).ToList(),
                Summary = request.Summary,
                AdditionalInfo = request.AdditionalInfo,
                Quantity = (int)request.Quantity,
                RemainingDays = (int)remainingDaysAfterSubstraction,
                IsRequestClosed = isRequestClosed,
                IsRequestApprovable = approvable,
                EmployeeFullName = employee.FullName,
                EmployeeID = employee.Id,
                RequestID = request.Id
            };

            return PartialView("_ManagerOoOSummary", requestModel);
        }

        public IActionResult RequestEquipmentManagerDetails(int requestId)
        {
            var request = _dbContext.Request.Where(r => r.Id == requestId.ToString()).FirstOrDefault();

            var employee = _dbContext.Employee.Where(e => e.Id == request.EmployeeId).FirstOrDefault();

            var isRequestClosed = request.StatusId == 3 || request.StatusId == 4 ? true : false;

            var requestModel = new RequestEquipmentManagerViewModel
            {
                RequestTypeID = request.RequestTypeID,
                RequestTypes = _dbContext.RequestType.Where(rt => rt.Id != 1 && rt.Id != 2).ToList(),
                Summary = request.Summary,
                AdditionalInfo = request.AdditionalInfo,
                Quantity = (int)request.Quantity,
                RemainingDays = 0,
                IsRequestClosed = isRequestClosed,
                IsRequestApprovable = true,
                EmployeeFullName = employee.FullName,
                EmployeeID = employee.Id,
                RequestID = request.Id
            };

            return PartialView("_ManagerEquipmentSummary", requestModel);
        }

        public List<RequestManagementWrapperModel> setRequestTableModel()
        {
            string loggedInUserId = User.Identity.Name;

            Employee loggedInEmployee = this._dbContext.Employee.FirstOrDefault(e => e.UserId == loggedInUserId);

            var teamId = loggedInEmployee.TeamId;

            var teamEmployees = _dbContext.Employee.Where(e => e.TeamId == teamId).ToList();

            var requests = _dbContext.Request
                .Where(r => teamEmployees.Select(t => t.Id).Contains(r.EmployeeId) && r.Status.Id != 1 && r.RequestTypeID == 1 || r.RequestTypeID == 2)
                .ToList();

            var wrapperModels = new List<RequestManagementWrapperModel>();

            foreach (var request in requests)
            {
                var requestEmployee = _dbContext.Employee.Where(e => e.Id == request.EmployeeId).FirstOrDefault();
                var requestModel = new RequestManagementWrapperModel
                {
                    Id = request.Id,
                    Summary = request.Summary,
                    Type = _dbContext.RequestType.Where(t => t.Id == request.RequestTypeID).FirstOrDefault()?.Name,
                    Status = _dbContext.Status.Where(s => s.Id == request.StatusId).FirstOrDefault()?.Name,
                    StatusId = (int)(_dbContext.Status.Where(s => s.Id == request.StatusId).FirstOrDefault()?.Id),
                    CreatedDate = request.CreatedDate,
                    EmployeeFullName = requestEmployee.FullName
                };

                wrapperModels.Add(requestModel);
            }

           return wrapperModels = wrapperModels.OrderBy(w => w.StatusId).ThenBy(w => requests.First(r => r.Id == w.Id).CreatedDate).ToList();
        }

        public EquipmentManagerWrapperModel setEquipmentForManagerTabModel()
        {
            string loggedInUserId = User.Identity.Name;

            Employee loggedInEmployee = this._dbContext.Employee.FirstOrDefault(e => e.UserId == loggedInUserId);

            var assignedEquipmentIds = new List<int>();

            foreach (var id in loggedInEmployee.EquipmentId.Split('#'))
            {
                assignedEquipmentIds.Add(int.Parse(id));
            };

            var assignedEquipment = _dbContext.Equipment.Where(e => assignedEquipmentIds.Contains(e.Id)).ToList();

            var teamId = loggedInEmployee.TeamId;

            var teamEmployees = _dbContext.Employee.Where(e => e.TeamId == teamId).ToList();

            var requests = _dbContext.Request
                .Where(r => teamEmployees.Select(t => t.Id).Contains(r.EmployeeId) && r.Status.Id != 1 && r.RequestTypeID != 1 && r.RequestTypeID != 2)
                .ToList();

            foreach (var eq in assignedEquipment)
            {
                eq.EquipmentCategory = _dbContext.EquipmentCategory.Where(c => c.Id == eq.CategoryId).FirstOrDefault();
            }

            var requestModels = new List<EquipmentManagerRequestModel>();

            foreach (var req in requests)
            {
                var requestEmployee = _dbContext.Employee.Where(e => e.Id == req.EmployeeId).FirstOrDefault();
                var requestModel = new EquipmentManagerRequestModel
                {                    
                    Id = req.Id,
                    Summary = req.Summary,
                    Type = _dbContext.RequestType.Where(t => t.Id == req.RequestTypeID).FirstOrDefault()?.Name,
                    Status = _dbContext.Status.Where(s => s.Id == req.StatusId).FirstOrDefault()?.Name,
                    StatusId = (int)(_dbContext.Status.Where(s => s.Id == req.StatusId).FirstOrDefault()?.Id),
                    CreatedDate = req.CreatedDate,
                    EmployeeFullName = requestEmployee.FullName
                    
                };
                requestModels.Add(requestModel);
            }

            var wrapperModels = new EquipmentManagerWrapperModel
            {
                Equipment = assignedEquipment,
                EquipmentRequests = requestModels
            };



            return wrapperModels;
        }


    }


    //u partial view se može slati jedan item, pa je više podataka wrappano
    public class TeamManagementWrapperModel
    {
        public List<Team> TeamList { get; set; }
        public List<int> IntList { get; set; }

        public List<String> ManagerNames { get; set; }
    }

    public class TeamDetailsWrapperModel
    {
        public Team Team { get; set; }
        public List<Employee> Employees { get; set; }

    }

    public class EmployeeViewModel
    {
        public List<Employee> Employees { get; set; }
        public Dictionary<int, List<string>> EquipmentNamesDictionary { get; set; }
    }

    public class ClockInViewModel
    {
        public List<Employee> Employees { get; set; }
        public List<ClockIn> ClockIns { get; set; }

    }

    public class TeamViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ManagerId { get; set; }
        public List<int> SelectedEmployeeIds { get; set; }
        public List<Employee> Managers { get; set; }
        public List<Employee> AvailableEmployees { get; set; }
    }

    public class RequestManagementWrapperModel
    {
        public string Id { get; set; }
        public string Summary { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
        public string EmployeeFullName { get; set; }

        public DateTime CreatedDate { get; set; }

    }

    public class RequestOoOManagerViewModel
    {
        public string RequestID { get; set; }
        public int RequestTypeID { get; set; }

        public List<RequestType> RequestTypes { get; set; }

        public string Summary { get; set; }

        public string AdditionalInfo { get; set; }
        public int Quantity { get; set; }
        public int RemainingDays { get; set; }
        
        public bool IsRequestClosed { get; set; }
        public bool IsRequestApprovable { get; set; }

        public bool? IsApproved { get; set; }
        public bool? IsRejected { get; set; }
        public string? Comment { get; set; }

        public string EmployeeFullName { get; set; }
        public int EmployeeID { get; set; }
    }

    public class EquipmentManagerWrapperModel
    {
        public List<Equipment> Equipment { get; set; }

        public List<EquipmentManagerRequestModel> EquipmentRequests { get; set; }

    }

    public class EquipmentManagerRequestModel
    {
        public string Id { get; set; }
        public string Summary { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
        public string EmployeeFullName { get; set; }

        public DateTime CreatedDate { get; set; }


    }

    public class RequestEquipmentManagerViewModel
    {
        public string RequestID { get; set; }
        public int RequestTypeID { get; set; }

        public List<RequestType> RequestTypes { get; set; }

        public string Summary { get; set; }

        public string AdditionalInfo { get; set; }
        public int Quantity { get; set; }
        public int RemainingDays { get; set; }

        public bool IsRequestClosed { get; set; }
        public bool IsRequestApprovable { get; set; }

        public bool? IsApproved { get; set; }
        public bool? IsRejected { get; set; }
        public string? Comment { get; set; }

        public string EmployeeFullName { get; set; }
        public int EmployeeID { get; set; }
    }
}
