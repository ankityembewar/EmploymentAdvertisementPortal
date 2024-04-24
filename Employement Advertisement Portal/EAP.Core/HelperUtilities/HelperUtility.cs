using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

namespace EAP.Core.HelperUtilities
{
    public class HelperUtility
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HelperUtility(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
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
                throw new InvalidOperationException("Employee ID claim is missing or invalid.");
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
    }
}
