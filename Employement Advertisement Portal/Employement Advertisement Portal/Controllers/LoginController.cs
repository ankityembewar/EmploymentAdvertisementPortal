using EAP.BAL.IAgent.IEmployee;
using EAP.BAL.IAgent.ILogin;
using EAP.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
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
        [HttpGet]
        public ActionResult UserLogin(bool showNotification = false)
        {
            if (showNotification)
            {
                ViewBag.NotificationMessage = "A password reset link has been sent to your email address.";
            }
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
                return RedirectToAction(isAdmin ? "Index" : "List", isAdmin ? "Admin" : "Advertisement");
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

        #region Forget Password
        [HttpGet]
        public ActionResult ForgotPassword(bool showNotification = false, string error="")
        {
            if (showNotification)
            {
                ViewData["ErrorMessage"] = error;
            }
            return View();
        }
        [HttpPost]
        public ActionResult  RequestPasswordReset(string email)
        {
            bool isEmployeeExists = _loginAgent.IsValidEmail(email);
            // Check if the email exists in the system
            if (isEmployeeExists)
            {
                string token = _loginAgent.ResetPassword(email);
                return RedirectToAction("UserLogin", new { showNotification = true });

            }
            else
            {
                return RedirectToAction("ForgotPassword");
            }
        }

        public ActionResult CreatePassword(string token, string email)
        {
            LoginViewModel login = new LoginViewModel();
            login.Token = token;
            login.Email = email;
            return View();
        }

        [HttpPost]
        public ActionResult CreatePassword(LoginViewModel login)
        {
            try
            {
                if (_loginAgent.CheckCredForForgotPassword(login.Email, login.Token))
                {
                    bool result = _loginAgent.UpdatePassword(login);
                    return UserLogin(login);
                }
                return View();
            }
            catch (Exception ex)
            { 
                return RedirectToAction("ForgotPassword", new { showNotification = true,error= ex.Message });
            }

        }



        #endregion
    }
}
