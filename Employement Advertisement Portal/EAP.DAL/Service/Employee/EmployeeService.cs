﻿using EAP.Core.Data;
using EAP.Core.HelperUtilities;
using EAP.DAL.IService.Employee;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace EAP.DAL.Service.Employee
{
    public class EmployeeService : IEmployeeService
    {
        #region Private Variables
        private readonly HelperUtility _helperUtility;
        #endregion

        public EmployeeService(HelperUtility helperUtility)
        {
            _helperUtility = helperUtility;
        }

        public EmployeeDetailsTbl GetEmployeeByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email can't be null or empty", nameof(email));
            }

            using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
            {
                EmployeeDetailsTbl employee = context.EmployeeDetailsTbls.Include(e=>e.Role).FirstOrDefault(x => x.Email == email);
                if (employee == null)
                {
                    throw new Exception("Employee with the specified email is not found");
                }

                return employee;
            }
        }


        public EmployeeDetailsTbl GetEmployeeInfo(int empId)
        {
            using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
            {
                return context.EmployeeDetailsTbls.Include(e=>e.Role).FirstOrDefault(x => x.EmpId == empId);
            }
        }

        public List<EmployeeDetailsTbl> GetEmployeeList()
        {
            using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
            {
                return context.EmployeeDetailsTbls.Include(e => e.Role).ToList();
            }
        }

        public List<UserRoleTbl> GetEmployeeRoleOptions()
        {
            using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
            {
                return context.UserRoleTbls.ToList();
            }
        }

        public SmtpSetting GetSMTPCred(int id=1)
        {
            using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
            {
                return context.SmtpSettings.FirstOrDefault(x => x.Id == id);
            }
        }

        public bool IsDuplicateEmail(string email)
        {
            throw new NotImplementedException();
        }

        public bool IsEmployeeAdded(EmployeeDetailsTbl employee, string password)
        {
            try
            {
                using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
                {
                    // Map audit fields for employee
                    _helperUtility.MapAuditFields(employee, "CreatedBy", "ModifiedBy", "CreatedDate", "ModifiedDate", false);

                    // Create user login
                    UserLoginTbl userLogin = new UserLoginTbl
                    {
                        Password = password,
                        Email = employee.Email
                    };
                    _helperUtility.MapAuditFields(userLogin, "CreatedBy", "ModifiedBy", "CreatedDate", "ModifiedDate", false);

                    // Add employee and user login to context
                    context.EmployeeDetailsTbls.Add(employee);
                    //context.UserLoginTbls.Add(userLogin);

                    // Save changes to commit both employee and user login
                    context.SaveChanges();

                    string subject = "Welcome! Here's Your Employee Account Information";
                    string body = "<html>\r\n<head>\r\n    <title>Welcome</title>\r\n</head>\r\n<body>\r\n    <h1>Welcome Aboard!</h1>\r\n    <p>We're thrilled to have you as part of our team. Your employee account has been successfully created. Below are your account details:</p>\r\n    <ul>\r\n        <li><strong>Username:</strong> [USERNAME]</li>\r\n        <li><strong>Email:</strong> [EMAIL]</li>\r\n    </ul>\r\n    <p>Please keep this information secure and do not share it with anyone.</p>\r\n    <p>To get started, please follow the link below to activate your account and set up your password:</p>\r\n    <a href=\"[SITE_URL]/Login/ForgetPassword\">Activate Account</a>\r\n    <p>If you have any questions, feel free to reach out to your manager or HR department.</p>\r\n    <p>Welcome once again, and we look forward to working with you!</p>\r\n    <p>This is an automated email; please do not reply to this message.</p>\r\n</body>\r\n</html>";
                    SendEmail(employee.Email,"ankit@yopmail.com", subject, body);
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                throw new InvalidOperationException(ex.Message);
            }
        }


        public bool IsEmployeeDeleted(int empId)
        {
            using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
            {
                // Retrieve the employee from the database
                EmployeeDetailsTbl employeeToDelete = context.EmployeeDetailsTbls.FirstOrDefault(x => x.EmpId == empId);

                if (employeeToDelete != null)
                {
                    // Remove related records from AdvertisementDetails_tbl
                    //var relatedAds = context.AdvertisementDetailsTbls.Where(ad => ad.EmpId == empId);
                    //context.AdvertisementDetailsTbls.RemoveRange(relatedAds);

                    // Remove the employee from the context
                    context.EmployeeDetailsTbls.Remove(employeeToDelete);

                    // Save changes to persist the deletion
                    context.SaveChanges();

                    return true; // Employee successfully deleted
                }

                return false; // Employee not found or deletion failed
            }
        }

        public bool IsSMPTPCredUpdate(SmtpSetting smtp)
        {
            using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
            {
                context.Entry(smtp).State = EntityState.Modified;
                context.SaveChanges();
                return true;
            }
        }

        public bool UpdateEmployeeInfo(EmployeeDetailsTbl employee)
        {
            try
            {
                using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
                {
                    _helperUtility.MapAuditFields(employee, "CreatedBy", "ModifiedBy", "CreatedDate", "ModifiedDate", true);
                    context.Entry(employee).State = EntityState.Modified;
                    context.SaveChanges();
                    return true;
                }
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }

        public void SendEmail(string to, string from, string subject, string body)
        {
            try
            {
                SmtpSetting smtp = GetSMTPCred();

                // Create a new SmtpClient instance using the provided SMTP settings
                using (var smtpClient = new SmtpClient(smtp.Host, smtp.Port))
                {
                    // Configure the SMTP client
                    smtpClient.Credentials = new NetworkCredential(smtp.Username, smtp.Password);
                    smtpClient.EnableSsl = smtp.EnableSsl;
                    smtpClient.Timeout = 30000;
                    smtpClient.UseDefaultCredentials = false;

                    // Create a new MailMessage instance
                    using (var mailMessage = new MailMessage(from, to))
                    {
                        mailMessage.Subject = subject;
                        mailMessage.Body = body;
                        mailMessage.IsBodyHtml = true;
                        // Send the email
                        smtpClient.Send(mailMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during email sending
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
   