using EAP.ViewModel;

namespace EAP.BAL.IAgent.ILogin
{
    public interface ILoginAgent
    {
        /// <summary>
        /// Validates the credentials provided in the login view model.
        /// </summary>
        /// <param name="login">The login view model containing the email and password.</param>
        /// <returns>True if the credentials are valid; otherwise, false.</returns>
        bool IsValidCredential(LoginViewModel login);

        bool IsValidEmail(string email);

        string ResetPassword(string email);

        bool CheckCredForForgotPassword(string email, string token);

        bool UpdatePassword(LoginViewModel login);
    }
}
