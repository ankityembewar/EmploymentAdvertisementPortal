using EAP.Core.Data;
using EAP.Core.HelperUtilities;
using EAP.Core.ResourceFile;
using EAP.DAL.IService.Login;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace EAP.DAL.Service.LoginService
{
    public class LoginService : ILoginService
    {
        #region Private Variables
        private readonly HelperUtility _helperUtility;
        #endregion

        public LoginService(HelperUtility helperUtility)
        {
            _helperUtility = helperUtility;
        }

        public bool CheckCredForForgotPassword(string email, string token)
        {
            try
            {
                using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
                {
                    if (context.UserLoginTbls.Any(x => x.Email == email && x.Token==token && x.TokenExpiryTime > DateTime.Now &&
                                     x.TokenExpiryTime <= DateTime.Now.AddMinutes(5)))
                        return true;
                    else
                        throw new Exception("Invalid credentials or token.");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

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

        public bool IsValidEmail(string email)
        {
            try
            {
                using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
                {
                    if (context.UserLoginTbls.Any(x => x.Email == email))
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

        public bool ResetPassword(string email, string token)
        {
            try
            {
                using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
                {
                    if (context.UserLoginTbls.Any(x => x.Email == email))
                    {
                        UserLoginTbl userLogin = context.UserLoginTbls.First(x => x.Email == email);
                        userLogin.Token = token;
                        userLogin.TokenExpiryTime = DateTime.Now.AddMinutes(5);
                        context.Entry(userLogin).State = EntityState.Modified;
                        context.SaveChanges();
                        

                        string subject = "Password Reset Request";
                        string body = EmailTemplate.ResetPassword;
                        body = body.Replace("{userLogin.Token}", userLogin.Token)
                               .Replace("{userLogin.Email}", userLogin.Email);
                        _helperUtility.SendEmail(userLogin.Email, "ankit@yopmail.com", subject, body);

                        return true;
                    }
                    else
                        throw new Exception("Invalid email or password.");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool UpdatePassword(UserLoginTbl login)
        {

            try
            {
                using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
                {
                    UserLoginTbl userLogin = context.UserLoginTbls.First(x => x.Email == login.Email && x.Token == login.Token);
                    if (userLogin != null)
                    {
                        // Update password
                        userLogin.Password = login.Password;

                        // Update other properties if needed
                        userLogin.ModifiedBy = 2;
                        userLogin.ModifiedDate = DateTime.Now;

                        context.SaveChanges();
                        return true;
                    }
                    else
                        throw new Exception("Update Password Failed.");
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
