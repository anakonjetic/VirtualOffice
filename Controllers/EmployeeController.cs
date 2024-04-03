using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VirtualOffice.Models;

namespace VirtualOffice.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult EmployeeHomePage()
        {
            return View();
        }

        public IActionResult LoadPartialView(string target)
        {
            
            switch (target)
            {
                case "home":
                    return PartialView("_EmployeeHome");
                case "clockIn":
                    return PartialView("_EmployeeClockIn"); 
                case "evaluation":
                    return PartialView("_EmployeeEvaluation");
                case "equipment":
                    return PartialView("_EmployeeEquipment");
                case "outOfOffice":
                    return PartialView("_EmployeeOutOfOffice");

                default:
                    return NotFound();
            }
        }

    }


}
