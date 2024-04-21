using EAP.BAL.IAgent.IEmployee;
using EAP.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Employement_Advertisement_Portal.Controllers
{
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
        #endregion
    }
}
