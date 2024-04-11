using EAP.Core.Data;
using EAP.DAL.IService.Login;

namespace EAP.DAL.Service.LoginService
{
    public class LoginService : ILoginService
    {
        #region Method
        public bool IsValidCredential(UserLoginTbl login)
        {
            try
            {
                using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
                {
                    if (context.UserLoginTbls.Any(x => x.Email == login.Email && x.Password == login.Password))
                        return true;
                    else
                        throw new Exception("Invalid email or password.");
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion
    }
}
