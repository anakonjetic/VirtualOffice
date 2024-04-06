using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Diagnostics;
using VirtualOffice.Data;
using VirtualOffice.Models;

namespace VirtualOffice.Controllers
{
    public class ManagerController : Controller
    {

        private ApplicationDbContext _dbContext;
        private DateTime? clockInTime; // Variable to store clock in time

        public ManagerController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
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
                    return PartialView("_ManagerOutOfOffice");
                case "equipment":
                    return PartialView("_ManagerEquipmentManagement");
                case "clock":
                    return PartialView("_ManagerClockIn", clockIn);
                case "hierarchy":
                    return PartialView("_ManagerHierarchy");
                case "team":
                    return PartialView("_ManagerTeamTable", teamManagementModel);
                case "export":
                    return PartialView("_ManagerDataExport", dataExport);

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

            return PartialView("_TeamCreate", team);
        }

        public IActionResult TeamCreate()
        {
            var managers = _dbContext.Employee.ToList();

            ViewData["Managers"] = managers;

            return PartialView("_TeamCreate", null);
        }

        public IActionResult SaveTeam(int id)
        {
            Team team;

            if (id == 0) {
                team = new Team();

                var teamName = Request.Form["Name"];
                team.Name = teamName;               

                var selectedManagerId = int.Parse(Request.Form["Employee"]);

                var teamEmployees = new List<Employee>();

                team.Employee = teamEmployees;

                var employeeManager = new EmployeeManager
                {
                    ManagerId = selectedManagerId,
                    EmployeeId = selectedManagerId
                };

                _dbContext.Team.Add(team);
                _dbContext.EmployeeManager.Add(employeeManager);

                _dbContext.EmployeeManager.Add(employeeManager);

            }
            else
            {
                team = _dbContext.Team
                       .Include(t => t.Employee)
                       .FirstOrDefault(t => t.Id == id);

                var teamName = Request.Form["Name"];
                team.Name = teamName;

                var selectedManagerId = int.Parse(Request.Form["Employee"]);

                var previousManagerId = _dbContext.Employee
                        .Where(e => e.TeamId == team.Id && _dbContext.EmployeeManager.Any(em => em.ManagerId == e.Id))
                        .Select(e => e.Id)
                        .SingleOrDefault();

                if (selectedManagerId != 0)
                {
                    if (previousManagerId != null && previousManagerId != selectedManagerId)
                    {
                        var employeesToRemove = _dbContext.EmployeeManager
                            .Where(em => em.ManagerId == previousManagerId && _dbContext.Employee.Any(e => e.Id == em.EmployeeId && e.TeamId == team.Id))
                            .ToList();

                        _dbContext.EmployeeManager.RemoveRange(employeesToRemove);

                        var teamEmployees = _dbContext.Employee.Where(e => e.TeamId == team.Id).ToList();


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
                }
            }

            _dbContext.SaveChanges();

            return View("ManagerHomePage", "team");
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

   
}
