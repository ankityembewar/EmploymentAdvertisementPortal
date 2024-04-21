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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public bool UpdateEmployeeInfo(EmployeeDetailsTbl employee)
        {
            throw new NotImplementedException();
        }
    }
}
