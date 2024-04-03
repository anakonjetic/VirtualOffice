using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VirtualOffice.Data;
using VirtualOffice.Models;

namespace VirtualOffice.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserManager<IdentityUser> _userManager;
        private ApplicationDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        //redirectanje do Manager/Employee ekrana
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var loggedInUserId = User.Identity.Name;
                var employee = _dbContext.Employee.SingleOrDefault(e => e.UserId == loggedInUserId);
                var isManager = employee?.Id != null && _dbContext.EmployeeManager.Any(em => em.ManagerId == employee.Id);

                if (isManager)
                {
                    return RedirectToAction("ManagerHomePage", "Manager");
                }
                else
                {
                    return RedirectToAction("EmployeeHomePage", "Employee");
                }
            }
            else
            {
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}