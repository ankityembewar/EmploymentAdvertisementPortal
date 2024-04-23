using EAP.Core.Data;
using EAP.DAL.IService.Employee;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAP.DAL.Service.Employee
{
    public class EmployeeService : IEmployeeService
    {
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

        public bool IsDuplicateEmail(string email)
        {
            throw new NotImplementedException();
        }

        public bool IsEmployeeAdded(EmployeeDetailsTbl employee)
        {
            throw new NotImplementedException();
        }

        public bool IsEmployeeDeleted(int empId)
        {
            using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
            {
                // Retrieve the employee from the database
                EmployeeDetailsTbl employeeToDelete = context.EmployeeDetailsTbls.FirstOrDefault(x => x.EmpId == empId);

                if (employeeToDelete != null)
                {
                    // Remove related records from UserLogin_tbl
                    var relatedLogins = context.UserLoginTbls.Where(login => login.EmpId == empId);
                    context.UserLoginTbls.RemoveRange(relatedLogins);

                    // Remove related records from AdvertisementDetails_tbl
                    var relatedAds = context.AdvertisementDetailsTbls.Where(ad => ad.EmpId == empId);
                    context.AdvertisementDetailsTbls.RemoveRange(relatedAds);

                    // Remove the employee from the context
                    context.EmployeeDetailsTbls.Remove(employeeToDelete);

                    // Save changes to persist the deletion
                    context.SaveChanges();

                    return true; // Employee successfully deleted
                }

                return false; // Employee not found or deletion failed
            }
        }




        public bool UpdateEmployeeInfo(EmployeeDetailsTbl employee)
        {
            throw new NotImplementedException();
        }
    }
}
