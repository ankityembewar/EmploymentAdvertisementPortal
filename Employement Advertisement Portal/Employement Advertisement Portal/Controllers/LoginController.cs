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
            return View();
        }

        [HttpPost]
        public ActionResult UserLogin(LoginViewModel login) 
        {
            try
            {
                bool result = _loginAgent.IsValidCredential(login);
                if (result)
                {
                    EmployeeViewModel employee = _employeeAgent.GetEmployeeByEmail(login.Email);
                    SignInUser(employee.Email, employee.EmpId.ToString(), "Admin");
                    return RedirectToAction("Index", "Home");
                }
                else
                    return RedirectToAction("UserLogin", "Login");
            }
            catch(Exception ex)
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
        private void SignInUser(string email, string userId, string roleName)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, email),
            new Claim("UserId", userId),
            new Claim(ClaimTypes.Role, roleName)
        };

            var userIdentity = new ClaimsIdentity(claims, "login");
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            HttpContext.SignInAsync(userPrincipal, new AuthenticationProperties
            {
                IsPersistent = false,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
            }).GetAwaiter().GetResult();
        }

        #endregion
    }
}
