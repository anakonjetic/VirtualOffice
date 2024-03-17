using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualOffice.Data;
using VirtualOffice.Models;

namespace VirtualOffice.Controllers
{
    public class ManagerController : Controller
    {

        private ApplicationDbContext _dbContext;

        public ManagerController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
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
    }

    //u partial view se može slati jedan item, pa je više podataka wrappano
    public class TeamManagementWrapperModel
    {
        public List<Team> TeamList { get; set; }
        public List<int> IntList { get; set; }
    }
}
