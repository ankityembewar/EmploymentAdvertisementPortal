using AutoMapper;
using EAP.BAL.IAgent.ILogin;
using EAP.Core.Data;
using EAP.DAL.IService.ILoginService;
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
        #endregion

        #region Method

        public bool IsValidCredential(LoginViewModel login)
        {
            UserLoginTbl userLogin = _mapper.Map<UserLoginTbl>(login);
            bool result = _loginService.IsValidCredential(userLogin);
            return result;
        }

         #endregion

    }
}
    
