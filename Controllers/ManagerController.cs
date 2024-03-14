using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
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

        public IActionResult LoadPartialView(string target)
        {
            
            string loggedInUserId = User.Identity.Name; 

            Employee loggedInEmployee = this._dbContext.Employee.FirstOrDefault(e => e.UserId == loggedInUserId);

            if (loggedInEmployee == null)
            {
                return NotFound();
            }

            // Employee Management Model definition-- start
            var managedTeamsQuery = this._dbContext.EmployeeManager
              .Where(em => em.ManagerId == loggedInEmployee.Id)
              .Select(em => em.Employee.TeamId);

            var employeesQuery = this._dbContext.Employee
               .Where(e => managedTeamsQuery.Contains(e.TeamId))
               .Include(e => e.Team);


            var employeeModel = employeesQuery.ToList();
            // Employee Management Model definition-- end




            switch (target)
            {
                case "home":
                    return PartialView("_ManagerHome"); 
                case "employee":
                    return PartialView("_ManagerEmployeeTable", employeeModel);
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
                    return PartialView("_ManagerTeamTable");
                case "export":
                    return PartialView("_ManagerDataExport");

                default:
                    return NotFound(); 
            }
        }
    }
}
