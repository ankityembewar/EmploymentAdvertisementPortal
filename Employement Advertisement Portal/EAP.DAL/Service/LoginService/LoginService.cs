using EAP.Core.Data;
using EAP.DAL.IService.ILoginService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    return context.UserLoginTbls.Any(x => x.Email == login.Email && x.Password == login.Password);
                }
            }
            catch (Exception ex)
            { 
                throw new ApplicationException("An error occurred while validating credentials. Please try again later.", ex);
            }

        }
        #endregion
    }
}
