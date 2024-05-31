using EAP.BAL.IAgent.IAdvertisement;
using EAP.BAL.IAgent.IEmployee;
using EAP.Core.HelperUtilities;
using EAP.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Employement_Advertisement_Portal.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {

        #region Private Variables
        private readonly HelperUtility _helperUtility;
        private readonly IEmployeeAgent _employeeAgent;
        private readonly IAdvertisementAgent _advertisementAgent;
        #endregion

        #region Constructor
        public AdminController(IEmployeeAgent employeeAgent, HelperUtility helperUtility, IAdvertisementAgent advertisementAgent)
        {
            _employeeAgent = employeeAgent;
            _helperUtility = helperUtility;
            _advertisementAgent = advertisementAgent;
        }
        #endregion

        #region Method
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetEmployeeList()
        {
            List<EmployeeViewModel> employeesList = _employeeAgent.GetEmployeeList();
            var item = new JsonResult(employeesList);
            return item;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddEmployee()
        {
            EmployeeViewModel employeeViewModel = new EmployeeViewModel();
            employeeViewModel.EmployeeRole = _employeeAgent.GetEmployeeRoleOptions();
            employeeViewModel.Dob = DateTime.Now;
            return View("CreateEdit", employeeViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AddEmployee(EmployeeViewModel employeeViewModel)
        {
            ModelState.Remove("Password");
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            try
            {
                bool result = _employeeAgent.IsEmployeeAdded(employeeViewModel);

                if (result)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to add employee.");
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("CreateEdit", employeeViewModel);
            }

            // If any error occurred during the process, return the view with error messages
            return View("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditEmployee(int empId)
        {
            EmployeeViewModel employee = _employeeAgent.GetEmployeeInfo(empId);
            employee.EmployeeRole = _employeeAgent.GetEmployeeRoleOptions();
            return PartialView("CreateEdit", employee);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult EditEmployee(EmployeeViewModel employeeViewModel)
        {
            var invalidProperties = ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { Property = x.Key, Errors = x.Value.Errors.Select(e => e.ErrorMessage) });
            ModelState.Remove("Password");


            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            try
            {
                bool result = _employeeAgent.UpdateEmployeeInfo(employeeViewModel);

                if (result)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to add employee.");
                }
            }
            catch (Exception ex)
            {
                // Log any unexpected exceptions and set a generic error message
                ModelState.AddModelError("", ex.Message);
            }

            // If any error occurred during the process, return the view with error messages
            return View("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult DeleteEmployee(int empId)
        {
            try
            {
                bool result = _employeeAgent.IsEmployeeDeleted(empId);
                return Json(new { success = result });
            }
            catch(Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("Index");
            }
        }
        #endregion

        #region SMTP
        [Authorize(Roles = "Admin")]
        public ActionResult SMTPCred(int id=1)
        {
            SMTPViewModel smtp = _employeeAgent.GetSMTPCred(id);
            return View(smtp);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult SMTPCred(SMTPViewModel cred)
        {
            bool result = _employeeAgent.IsSMPTPCredUpdate(cred);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        #endregion

        #region Advertisement Request

        public ActionResult AdvertisementRequest()
        {
            return View();
        }

        public ActionResult GetAdvertisementRequest()
        {
            List<AdvertisementViewModel> advertisements = _advertisementAgent.GetAdvertisementRequestList();
            return new JsonResult(advertisements);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public bool ActionOnAdvertisement(int advId, string decision)
        {
            if (advId != 0 && decision != null)
                return _advertisementAgent.ActionOnAdvertisement(advId, decision);
            else
                return false;
        }

        public ActionResult Details(int empId)
        {
            EmployeeViewModel employee = _employeeAgent.GetEmployeeInfo(empId);
            employee.EmployeeRole = _employeeAgent.GetEmployeeRoleOptions();
            return View(employee);
        }
        #endregion
    }
}
