using EAP.Core.Data;
using EAP.Core.HelperUtilities;
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

        public bool CheckCredForForgetPassword(string email, string token)
        {
            try
            {
                using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
                {
                    if (context.UserLoginTbls.Any(x => x.Email == email && x.Token==token))
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
                        context.Entry(userLogin).State = EntityState.Modified;
                        context.SaveChanges();
                        

                        string subject = "Password Reset Request";
                        string body = "<html><head><title>Password Reset Request</title></head><body><h1>Password Reset Request</h1><p>We received a request to reset your account password.</p><p>If you did not make this request, please disregard this message.</p><p>To reset your password, please follow the link below:</p><a href=\"http:/localhost:5288/Login/CreatePassword?token=" + userLogin.Token +"&email="+userLogin.Email+ "\">Reset Password</a><p>If you have any questions or need further assistance, please contact your manager or HR department.</p><p>Thank you!</p><p>This is an automated email; please do not reply to this message.</p></body></html>";
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
