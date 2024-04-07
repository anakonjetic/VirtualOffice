using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;
using VirtualOffice.Data;
using VirtualOffice.Models;

namespace VirtualOffice.Controllers
{
    public class ManagerController : Controller
    {
        private readonly ILogger<ManagerController> _logger;
        private ApplicationDbContext _dbContext;
        private UserManager<IdentityUser> _userManager;

        public ManagerController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, ILogger<ManagerController> logger)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
            this._logger = logger;
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

            var teamManagementModel = new TeamManagementWrapperModel
            {
                TeamList = teamModel,
                IntList = managedTeamIds
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
                    return PartialView("_ManagerClockIn");
                case "hierarchy":
                    return PartialView("_ManagerHierarchy");
                case "team":
                    return PartialView("_ManagerTeamTable", teamManagementModel);
                case "export":
                    return PartialView("_ManagerDataExport");
                case "create":
                    return PartialView("_CreateEmployee");

                default:
                    return NotFound();
            }
        }

        public IActionResult TeamSummary(int teamId)
        {
            return PartialView("_TeamSummary", teamId);
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

            string loggedInUserId = User.Identity.Name;
            var employeeModel = GetEmployeeManagementModel(loggedInUserId);
            return View("ManagerHomePage", "employee");
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

            if(ok && this.ModelState.IsValid)
            {
                this._dbContext.SaveChanges();
                return View("ManagerHomePage", "employee");
            }

            return View("ManagerHomePage", "employee");
        }

        [HttpPost]
        public IActionResult Create(Employee model)
        {
            model.SickLeaveDaysUsed = 0;
            model.RemainingDaysOff = 25;

            if (ModelState.IsValid)
            {
                this._dbContext.Employee.Add(model);
                this._dbContext.SaveChanges();

                var manager = new EmployeeManager();
                if(model.TeamId == 4)
                {
                    manager.ManagerId = 14;
                }
                else if(model.TeamId == 5)
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
    }

    //u partial view se može slati jedan item, pa je više podataka wrappano
    public class TeamManagementWrapperModel
    {
        public List<Team> TeamList { get; set; }
        public List<int> IntList { get; set; }
    }
}
