using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VirtualOffice.Data;
using VirtualOffice.Models;
using Microsoft.Extensions.Logging;

namespace VirtualOffice.Controllers
{
    
    public class EmployeeController : Controller
    {

        private ApplicationDbContext _dbContext;
        private ILogger<ManagerController> _logger;

        public EmployeeController(ApplicationDbContext dbContext, ILogger<ManagerController> logger)
        {
            this._dbContext = dbContext;
            _logger = logger;
        }

        public IActionResult EmployeeHomePage()
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

            var employeeClockIns = GetClockInsByLoggedInEmployee(loggedInEmployee);

            var manager = GetManager(loggedInUserId);

            var evaluationType = GetEvaluationType();


            Console.WriteLine("Logged-in Employee:");
            Console.WriteLine($"Id: {loggedInEmployee.Id}");
            Console.WriteLine($"Name: {loggedInEmployee.FirstName} {loggedInEmployee.LastName}");

            var clockIn1 = new EmployeeClockInViewModel
            {
                Employee = loggedInEmployee,
                ClockIns = employeeClockIns

            };
            
            var requestModel = setRequestTableModel();
            var equipmentModel = setEquipmentTabModel();

            var evaluationModel2 = new ManagerEvaluationViewModel
            {
                Manager = manager,
                LoggedInEmployee = loggedInEmployee,
                EvaluationForm = new EvaluationForm(),
                EvaluationType = evaluationType
            };

            //dohvaćanje podataka za model poslan u partial view --end
            switch (target)
            {
                case "clockIn":
                    return PartialView("_EmployeeClockIn", clockIn1); 
                case "evaluation":
                    return PartialView("_EmployeeEvaluation", evaluationModel2);
                case "equipment":
                    return PartialView("_EmployeeEquipment", equipmentModel);
                case "outOfOffice":
                    return PartialView("_EmployeeOutOfOffice", requestModel);
                case "settings":
                    return PartialView("_EmployeeEditAccount", loggedInEmployee);

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

        public IActionResult RequestEquipmentCreate()
        {
            var requestEquipmentViewModel = new RequestEquipmentViewModel
            {
                RequestTypes = _dbContext.RequestType.Where(r => r.Id != 1 && r.Id != 2).ToList(),

            };

            return PartialView("_EmployeeCreateEquipmentRequest", requestEquipmentViewModel);
        }

        public IActionResult SaveEquipmentRequest(RequestEquipmentViewModel model)
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

            return PartialView("EmployeeHomePage", "equipment");
        }

        public IActionResult SendEquipmentRequestToApproval(int requestId)
        {
            var requestToUpdate = _dbContext.Request.Where(r => r.Id == requestId.ToString()).FirstOrDefault();

            if (requestToUpdate != null)
            {
                requestToUpdate.StatusId = 2;
                _dbContext.SaveChanges();
            }

            var requestModel = setEquipmentTabModel();

            return PartialView("_EmployeeEquipment", requestModel);

        }

        public IActionResult DeleteEquipmentRequest(int requestId)
        {
            var requestToUpdate = _dbContext.Request.Where(r => r.Id == requestId.ToString()).FirstOrDefault();

            if (requestToUpdate != null)
            {
                _dbContext.Remove(requestToUpdate);
                _dbContext.SaveChanges();
            }

            var requestModel = setEquipmentTabModel();

            return PartialView("_EmployeeEquipment", requestModel);
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
                RequestTypes = _dbContext.RequestType.Where(rt => rt.Id == 1 || rt.Id == 2).ToList(),
                Summary = request.Summary,
                AdditionalInfo = request.AdditionalInfo,
                Quantity = (int)request.Quantity
            };

            return PartialView("_EmployeeOoOSummary", requestModel);
        }

        public IActionResult RequestEquipmentDetails(int requestId)
        {
            var request = _dbContext.Request.Where(r => r.Id == requestId.ToString()).FirstOrDefault();

            var requestModel = new RequestOoOViewModel
            {
                RequestTypeID = request.RequestTypeID,
                RequestTypes = _dbContext.RequestType.Where(rt => rt.Id != 1 && rt.Id != 2).ToList(),
                Summary = request.Summary,
                AdditionalInfo = request.AdditionalInfo,
                Quantity = (int)request.Quantity
            };

            return PartialView("_EmployeeEquipmentRequestSummary", requestModel);
        }

        public List<RequestWrapperModel> setRequestTableModel()
        {
            string loggedInUserId = User.Identity.Name;

            Employee loggedInEmployee = this._dbContext.Employee.FirstOrDefault(e => e.UserId == loggedInUserId);

            var requests = _dbContext.Request
                .Where(r => r.EmployeeId == loggedInEmployee.Id && r.RequestTypeID == 1 || r.RequestTypeID == 2)
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


        public EquipmentWrapperModel setEquipmentTabModel()
        {
            string loggedInUserId = User.Identity.Name;

            Employee loggedInEmployee = this._dbContext.Employee.FirstOrDefault(e => e.UserId == loggedInUserId);

            var assignedEquipmentIds = new List<int>();
                
              foreach (var id in loggedInEmployee.EquipmentId.Split('#'))
            {                
                assignedEquipmentIds.Add(int.Parse(id));
            };

            var assignedEquipment = _dbContext.Equipment.Where(e => assignedEquipmentIds.Contains(e.Id)).ToList();

            var requests = _dbContext.Request
                .Where(r => r.EmployeeId == loggedInEmployee.Id && r.RequestTypeID != 1 && r.RequestTypeID != 2)
                .ToList();

            foreach (var eq in assignedEquipment)
            {
                eq.EquipmentCategory = _dbContext.EquipmentCategory.Where(c => c.Id == eq.CategoryId).FirstOrDefault();
            }

            var requestModels = new List<EquipmentRequestModel>();

            foreach (var req in requests)
            {
                var requestModel = new EquipmentRequestModel
                {
                    Id = req.Id,
                    Summary = req.Summary,
                    Type = _dbContext.RequestType.Where(t => t.Id == req.RequestTypeID).FirstOrDefault()?.Name,
                    Status = _dbContext.Status.Where(s => s.Id == req.StatusId).FirstOrDefault()?.Name,
                    SendToApproval = _dbContext.Status.Where(s => s.Id == req.StatusId).FirstOrDefault()?.Name == "New" ? true : false
                };
                requestModels.Add(requestModel);
            }

            var wrapperModels = new EquipmentWrapperModel
            {
                Equipment = assignedEquipment,
                EquipmentRequests = requestModels
            };

                     

            return wrapperModels;
        }
               

        [HttpPost]
        public async Task<IActionResult> EditAccount(int id)
        {
            var model = this._dbContext.Employee.Single(d => d.Id == id);
            var ok = await this.TryUpdateModelAsync(model);

            if (ok && ModelState.IsValid)
            {
                this._dbContext.SaveChanges();
                return View("EmployeeHomePage");
            }

            return View("EmployeeHomePage");
        }

        private Employee GetManager(string loggedInUserId)
        {
            Employee loggedInEmployee = this._dbContext.Employee.FirstOrDefault(e => e.UserId == loggedInUserId);

            if (loggedInEmployee == null)
            {
                return null;
            }

            var managerQuery = this._dbContext.EmployeeManager
                .Where(em => em.EmployeeId == loggedInEmployee.Id)
                .Select(em => em.Manager);

            return managerQuery.FirstOrDefault();
        }

        private List<EvaluationType> GetEvaluationType()
        {
            return _dbContext.EvaluationType.ToList();
        }

        // GET: ManagerController/SubmitEvaluation
        [HttpPost]
        public IActionResult SubmitEvaluation2(ManagerEvaluationViewModel model)
        {

            ModelState.Clear();
            var evaluationForm = new EvaluationForm
            {
                EmployeeId = model.EmployeeId,
                ManagerId = model.ManagerId,
                FormTitle = model.EvaluationForm.FormTitle,
                FormDescription = model.EvaluationForm.FormDescription,
                Rating = model.EvaluationForm.Rating,
                Date = model.EvaluationForm.Date,
                EvaluationTypeId = model.EvaluationTypeId // Assuming EvaluationTypeId is the correct property name
            };

            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                _logger.LogInformation("TEST");

                try
                {
                    _dbContext.EvaluationForm.Add(evaluationForm);
                    _dbContext.SaveChanges();
                    return View("EmployeeHomePage", "evaluation");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the evaluation form.");
                    return View("EmployeeHomePage", "evaluation");

                }
            }
            else
            {
                return View("EmployeeHomePage", "evaluation");
            }
        }
     

        private List<ClockIn> GetClockInsByLoggedInEmployee(Employee loggedInEmployee)
        {
            return _dbContext.ClockIns
                .Where(c => c.EmployeeId == loggedInEmployee.Id)
                .ToList();
        }

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


    public class EmployeeClockInViewModel
    {
        public Employee? Employee { get; set; }
        public List<ClockIn> ClockIns { get; set; }

    }


    public class EquipmentWrapperModel
    {
        public List<Equipment> Equipment { get; set; }

        public List<EquipmentRequestModel> EquipmentRequests { get; set; }

    }

    public class EquipmentRequestModel
    {
        public string Id { get; set; }
        public string Summary { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }

        public bool SendToApproval { get; set; }

    }

    public class RequestEquipmentViewModel
    {
        public int RequestTypeID { get; set; }

        public List<RequestType> RequestTypes { get; set; }

        public string Summary { get; set; }

        public string AdditionalInfo { get; set; }
        public int Quantity { get; set; }
    }

    public class ManagerEvaluationViewModel
    {
        public Employee Manager { get; set; }
        public Employee LoggedInEmployee { get; set; }

        public EvaluationForm EvaluationForm { get; set; }

        public List<EvaluationType> EvaluationType { get; set; }

        public int EvaluationTypeId { get; set; }
        public int EmployeeId { get; set; }

        public int ManagerId { get; set; }

    }


}
