using EAP.BAL.IAgent.ILogin;
using EAP.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Employement_Advertisement_Portal.Controllers
{
    public class LoginController : Controller
    {
        #region Private Variables
        private readonly ILoginAgent _loginAgent;
        #endregion

        #region Constructor
        public LoginController(ILoginAgent loginAgent)
        {
            _loginAgent = loginAgent;
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
            bool result = _loginAgent.IsValidCredential(login);
            if(result)
                return RedirectToAction("Index", "Home");
            else
                return RedirectToAction("UserLogin", "Login");
        }
        #endregion
    }
}
