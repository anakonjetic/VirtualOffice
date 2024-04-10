using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VirtualOffice.Data;
using VirtualOffice.Models;

namespace VirtualOffice.Controllers
{
    
    public class EmployeeController : Controller
    {

        private ApplicationDbContext _dbContext;

        public EmployeeController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IActionResult EmployeeHomePage()
        {
            return View();
        }

        public IActionResult LoadPartialView(string target)
        {

            //dohvaćanje podataka za model poslan u partial view --start

            var requestModel = setRequestTableModel();

            //dohvaćanje podataka za model poslan u partial view --end

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
                    return PartialView("_EmployeeOutOfOffice", requestModel);

                default:
                    return NotFound();
            }
        }

        // POST: /Employee/ClockIn
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

        // POST: /Employee/ClockOut
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

        public IActionResult RequestCreate()
        {
            var requestOoOViewModel = new RequestOoOViewModel
            {
                RequestTypes = _dbContext.RequestType.Where(r => r.Id == 1 || r.Id == 2).ToList(),
                
            };

            return PartialView("_EmployeeCreateOoORequest", requestOoOViewModel);
        }

        public IActionResult SaveOoORequest(RequestOoOViewModel model)
        {
            string loggedInUserId = User.Identity.Name;
            Employee loggedInEmployee = this._dbContext.Employee.FirstOrDefault(e => e.UserId == loggedInUserId);

            var lastReq = _dbContext.Request.OrderByDescending(r => r.Id).FirstOrDefault();
            var reqId = 0;

            if (lastReq != null)
            {
                reqId = Int32.Parse(lastReq.Id) + 1;
            }
            else
            {
                reqId = 1;
            }

            var request = new Request
            {
                Id = reqId.ToString(),
                EmployeeId = loggedInEmployee.Id,
                CreatedDate = DateTime.Now,
                ManagerId = 1,
                StatusId = 1, /*NEW*/
                RequestTypeID = model.RequestTypeID,
                Summary = model.Summary,
                AdditionalInfo = model.AdditionalInfo,
                Quantity = model.Quantity
            };

            _dbContext.Request.Add(request);

            _dbContext.SaveChanges();

            return PartialView("EmployeeHomePage", "outOfOffice");
        }

        public IActionResult SendRequestToApproval(int requestId)
        {
            var requestToUpdate = _dbContext.Request.Where(r => r.Id == requestId.ToString()).FirstOrDefault();

            if (requestToUpdate != null)
            {
                requestToUpdate.StatusId = 2; /*In progress*/
                _dbContext.SaveChanges();
            }

            var requestModel = setRequestTableModel();

            return PartialView("_EmployeeOutOfOffice", requestModel);
        }

        public IActionResult DeleteOoORequest(int requestId)
        {
            var requestToUpdate = _dbContext.Request.Where(r => r.Id == requestId.ToString()).FirstOrDefault();

            if (requestToUpdate != null)
            {
                _dbContext.Remove(requestToUpdate);
                _dbContext.SaveChanges();
            }

            var requestModel = setRequestTableModel();

            return PartialView("_EmployeeOutOfOffice", requestModel);
        }

        public IActionResult RequestOoODetails(int requestId)
        {
            var request = _dbContext.Request.Where(r => r.Id == requestId.ToString()).FirstOrDefault();

            var requestModel = new RequestOoOViewModel
            {
                RequestTypeID = request.RequestTypeID,
                RequestTypes = _dbContext.RequestType.Where(rt => rt.Id == 1 || rt.Id == 2 || rt.Id == 3 || rt.Id == 4).ToList(),
                Summary = request.Summary,
                AdditionalInfo = request.AdditionalInfo,
                Quantity = (int)request.Quantity
            };

            return PartialView("_EmployeeOoOSummary", requestModel);
        }

        public List<RequestWrapperModel> setRequestTableModel()
        {
            string loggedInUserId = User.Identity.Name;

            Employee loggedInEmployee = this._dbContext.Employee.FirstOrDefault(e => e.UserId == loggedInUserId);

            var requests = _dbContext.Request
                .Where(r => r.EmployeeId == loggedInEmployee.Id)
                .ToList();

            var wrapperModels = new List<RequestWrapperModel>();

            foreach(var request in requests)
            {
                var requestModel = new RequestWrapperModel
                {
                    Id = request.Id,
                    Summary = request.Summary,
                    Type = _dbContext.RequestType.Where(t => t.Id == request.RequestTypeID).FirstOrDefault()?.Name,
                    Status = _dbContext.Status.Where(s => s.Id == request.StatusId).FirstOrDefault()?.Name,
                    SendToApproval = _dbContext.Status.Where(s => s.Id == request.StatusId).FirstOrDefault()?.Name == "New" ? true : false
                };

                wrapperModels.Add(requestModel);
            }

            return wrapperModels;
        }

        

        public class RequestWrapperModel
        {
            public string Id { get; set; }
            public string Summary { get; set; }
            public string Type { get; set; }
            public string Status { get; set; }

            public bool SendToApproval { get; set; }

        }
        
        public class RequestOoOViewModel
        {
            public int RequestTypeID { get; set; }

            public List<RequestType> RequestTypes { get; set; }

            public string Summary { get; set; }

            public string AdditionalInfo { get; set; }
            public int Quantity { get; set; }
        }

    }


}
