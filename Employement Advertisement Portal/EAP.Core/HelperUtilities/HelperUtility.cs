﻿using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using EAP.ViewModel;
using EAP.Core.Data;
using System.Net.Mail;
using System.Net;

namespace EAP.Core.HelperUtilities
{
    public class HelperUtility
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _environment;

        public HelperUtility(IHttpContextAccessor httpContextAccessor, IHostingEnvironment environment)
        {
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
        }

        public int GetEmployeeId()
        { 
            // Get the current user's claims principal from the HttpContext
            ClaimsPrincipal user = _httpContextAccessor.HttpContext.User;

            // Find the claim with the type "EmpId"
            Claim empIdClaim = user.FindFirst("EmpId");

            // Check if the claim exists and can be parsed as an int
            if (empIdClaim != null && int.TryParse(empIdClaim.Value, out int empId))
            {
                // Return the parsed value of the "EmpId" claim
                return empId;
            }
            else
            {
                // Handle the situation where the claim doesn't exist or can't be parsed as an int
                // You might throw an exception, return a default value, or handle it based on your application's requirements
                //throw new InvalidOperationException("Employee ID claim is missing or invalid.");
                _httpContextAccessor.HttpContext.Response.Redirect("/Login/UserLogin");
                return -1; // Return a default value or handle this case as required
            }
        }

        public bool IsUserLoggedIn()
        {
            // Check if there is an authenticated user in the HttpContext
            return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public void MapAuditFields<T>(T viewModel, string createdByPropertyName, string modifiedByPropertyName, string createdDatePropertyName, string modifiedDatePropertyName, bool isForUpdate)
        {
            // Get the employee ID from the current user's claims
            int empId = GetEmployeeId();

            // Check if empId is greater than 0
            if (empId > 0)
            {
                if (isForUpdate)
                {
                    // Set only the ModifiedBy and ModifiedDate properties
                    SetPropertyValueIfExists(viewModel, modifiedByPropertyName, empId);
                    SetPropertyValueIfExists(viewModel, modifiedDatePropertyName, DateTime.Now);
                }
                else
                {
                    // Set all audit fields for creation
                    SetPropertyValueIfExists(viewModel, createdByPropertyName, empId);
                    SetPropertyValueIfExists(viewModel, modifiedByPropertyName, empId);
                    SetPropertyValueIfExists(viewModel, createdDatePropertyName, DateTime.Now);
                    SetPropertyValueIfExists(viewModel, modifiedDatePropertyName, DateTime.Now);
                }
            }
            else
            {
                throw new InvalidOperationException("Employee ID is not valid.");
            }
        }

        private void SetPropertyValueIfExists<T>(T obj, string propertyName, object value)
        {
            // Check if the property exists in the model
            PropertyInfo property = typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (property != null && property.CanWrite)
            {
                // Set the property value
                property.SetValue(obj, value);
            }
            else
            {
                throw new ArgumentException($"Property '{propertyName}' not found or not writable in type '{typeof(T).Name}'.");
            }
        }

        public string SaveImage(IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                // Generate a unique file name to avoid conflicts
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = "Image/Advertisement/" + fileName;
                var fullPath = Path.Combine(_environment.WebRootPath, filePath);

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                // Save the uploaded image to the specified path
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }

                // Construct the relative URL of the uploaded image
                var relativeUrl = "/" + filePath.Replace('\\', '/'); // Assuming images are served from the root directory

                return relativeUrl;
            }
            return "";
        }

        public bool IsUserAuthorizedToEdit(int loginUserId, string userRole, EmployeeViewModel employee)
        {
            // Check if the user has an admin role
            if (string.Equals(userRole, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            // Check if the user is an employee editing their own record
            if (loginUserId == employee?.EmpId && employee?.EmployeeRole?.Any()!=null)
            {
                string employeeRole = employee.EmployeeRole.First().Text;
                if (string.Equals(userRole, employeeRole, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            // User is not authorized
            return false;
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
                    using (var mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(from, "EAP Portal");
                        mailMessage.To.Add(to);
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

        public SmtpSetting GetSMTPCred(int id = 1)
        {
            using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
            {
                return context.SmtpSettings.FirstOrDefault(x => x.Id == id);
            }
        }
    }
}
