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

    }
}
