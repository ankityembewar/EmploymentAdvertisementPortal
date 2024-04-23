using EAP.BAL.IAgent.IEmployee;
using EAP.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employement_Advertisement_Portal.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {

        #region Private Variables
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmployeeAgent _employeeAgent;
        #endregion

        #region Constructor
        public AdminController(IEmployeeAgent employeeAgent, IHttpContextAccessor httpContextAccessor)
        {
            _employeeAgent = employeeAgent;
            _httpContextAccessor = httpContextAccessor;
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

        public ActionResult AddEmployee(EmployeeViewModel employeeViewModel)
        {

            employeeViewModel.EmployeeRole = _employeeAgent.GetEmployeeRoleOptions();
            employeeViewModel.Dob = DateTime.Now;
            return View("CreateEdit", employeeViewModel);
        }

        public ActionResult EditEmployee(int empId)
        {
            EmployeeViewModel employee = _employeeAgent.GetEmployeeInfo(empId);
            employee.EmployeeRole = _employeeAgent.GetEmployeeRoleOptions();
            return PartialView("CreateEdit", employee);
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
