using EAP.BAL.IAgent.IEmployee;
using EAP.BAL.IAgent.ILogin;
using EAP.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Employement_Advertisement_Portal.Controllers
{
    public class LoginController : Controller
    {
        #region Private Variables
        private readonly ILoginAgent _loginAgent;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmployeeAgent _employeeAgent;
        #endregion

        #region Constructor
        public LoginController(ILoginAgent loginAgent, IEmployeeAgent employeeAgent, IHttpContextAccessor httpContextAccessor)
        {
            _loginAgent = loginAgent;
            _employeeAgent = employeeAgent;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Login
        public ActionResult UserLogin()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult UserLogin(LoginViewModel login) 
        {
            try
            {
                if (!_loginAgent.IsValidCredential(login))
                    return RedirectToAction("UserLogin", "Login");

                EmployeeViewModel employee = _employeeAgent.GetEmployeeByEmail(login.Email);
                if (employee == null || employee.EmployeeRole == null)
                    return RedirectToAction("Index", "Home");

                bool isAdmin = employee.EmployeeRole.Any(role => role.Text == "Admin");
                SignInUser(login.Email, employee.EmpId.ToString(), employee.EmployeeRole.Select(x => x.Text).First());
                return RedirectToAction(isAdmin ? "Index" : "Index", isAdmin ? "Admin" : "Home");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("UserLogin");
            }
            
        }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            HttpContext.SignOutAsync();

            _httpContextAccessor?.HttpContext?.Session.Clear();

            return RedirectToAction("UserLogin","Login");
        }
        #endregion

        #region Private Method
        private void SignInUser(string email, string empId, string roleName)
        {
            List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, email),
            new Claim("EmpId", empId),
            new Claim(ClaimTypes.Role, roleName)
        };

            ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");
            ClaimsPrincipal userPrincipal = new ClaimsPrincipal(userIdentity);

            HttpContext.SignInAsync(userPrincipal, new AuthenticationProperties
            {
                IsPersistent = false,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
            }).GetAwaiter().GetResult();
        }

        #endregion
    }
}
