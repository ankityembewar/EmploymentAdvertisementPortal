using AutoMapper;
using EAP.BAL.IAgent.ILogin;
using EAP.Core.Data;
using EAP.DAL.IService.Login;
using EAP.ViewModel;

namespace EAP.BAL.Agent.Login
{
    public class LoginAgent : ILoginAgent
    {
        #region Private Variables
        private readonly IMapper _mapper;
        private readonly ILoginService _loginService;
        #endregion

        #region Constructor
        public LoginAgent(IMapper mapper, ILoginService loginService)
        {
            _mapper = mapper;
            _loginService = loginService;
        }

        public bool CheckCredForForgetPassword(string email, string token)
        {
            return _loginService.CheckCredForForgetPassword(email, token);
        }
        #endregion

        #region Method

        public bool IsValidCredential(LoginViewModel login)
        {
            UserLoginTbl userLogin = _mapper.Map<UserLoginTbl>(login);
            bool result = _loginService.IsValidCredential(userLogin);
            return result;
        }

        public bool IsValidEmail(string email)
        {
            return _loginService.IsValidEmail(email);
        }

        public string ResetPassword(string email)
        {
            string token = Guid.NewGuid().ToString();
            bool result = _loginService.ResetPassword(email, token);
            if (result)
            {
                return token;
            }
            else
            {
                return string.Empty;
            }
        }

        public bool UpdatePassword(LoginViewModel login)
        {
            UserLoginTbl userLogin = _mapper.Map<UserLoginTbl>(login);
            bool result = _loginService.UpdatePassword(userLogin);
            return result;
        }

        #endregion

    }
}
    
