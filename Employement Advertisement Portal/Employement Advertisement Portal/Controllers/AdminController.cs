using EAP.BAL.IAgent.IEmployee;
using EAP.Core.HelperUtilities;
using EAP.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Employement_Advertisement_Portal.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {

        #region Private Variables
        private readonly HelperUtility _helperUtility;
        private readonly IEmployeeAgent _employeeAgent;
        #endregion

        #region Constructor
        public AdminController(IEmployeeAgent employeeAgent, HelperUtility helperUtility)
        {
            _employeeAgent = employeeAgent;
            _helperUtility = helperUtility;
        }
        #endregion

        #region Method

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetEmployeeList()
        {
            List<EmployeeViewModel> employeesList = _employeeAgent.GetEmployeeList();
            return new JsonResult(employeesList);
        }

        public ActionResult AddEmployee()
        {
            EmployeeViewModel employeeViewModel = new EmployeeViewModel();
            employeeViewModel.EmployeeRole = _employeeAgent.GetEmployeeRoleOptions();
            employeeViewModel.Dob = DateTime.Now;
            return View("CreateEdit", employeeViewModel);
        }

        [HttpPost]
        public ActionResult AddEmployee(EmployeeViewModel employeeViewModel)
        {
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


        public ActionResult EditEmployee(int empId)
        {
            EmployeeViewModel employee = _employeeAgent.GetEmployeeInfo(empId);
            employee.EmployeeRole = _employeeAgent.GetEmployeeRoleOptions();
            return PartialView("CreateEdit", employee);
        }

        [HttpPost]
        public ActionResult EditEmployee(EmployeeViewModel employeeViewModel)
        {
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
    }
}
