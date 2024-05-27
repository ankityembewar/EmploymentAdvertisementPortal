﻿using EAP.Core.Data;
using EAP.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAP.DAL.IService.Login
{
    public interface ILoginService
    {
        /// <summary>
        /// Validates the credentials provided in the login view model.
        /// </summary>
        /// <param name="login">The UserLoginTbl model containing the email and password.</param>
        /// <returns>True if the credentials are valid; otherwise, false.</returns>
        bool IsValidCredential(UserLoginTbl login);

        bool IsValidEmail(string email);

        bool ResetPassword(string email, string token);

        bool CheckCredForForgotPassword(string email, string token);

        bool UpdatePassword(UserLoginTbl login);
    }
}
