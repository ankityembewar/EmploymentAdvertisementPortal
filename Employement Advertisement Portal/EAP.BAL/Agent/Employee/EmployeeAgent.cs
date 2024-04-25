using AutoMapper;
using EAP.BAL.IAgent.IEmployee;
using EAP.Core.Data;
using EAP.DAL.IService.Employee;
using EAP.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EAP.BAL.Agent.Employee
{
    public class EmployeeAgent : IEmployeeAgent
    {
        #region Private Variables
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;
        #endregion

        #region Constructor
        public EmployeeAgent(IMapper mapper, IEmployeeService employeeService)
        {
            _mapper = mapper;
            _employeeService = employeeService;
        }
        #endregion

        #region Method
        public EmployeeViewModel GetEmployeeByEmail(string email)
        {
            EmployeeDetailsTbl employeeDetails = _employeeService.GetEmployeeByEmail(email);
            if (employeeDetails == null)
            {
                throw new InvalidOperationException("Employee details not found");
            }

            return _mapper.Map<EmployeeViewModel>(employeeDetails);
        }


        public EmployeeViewModel GetEmployeeInfo(int empId)
        {
            EmployeeDetailsTbl employeeDetails = _employeeService.GetEmployeeInfo(empId);
            return _mapper.Map<EmployeeViewModel>(employeeDetails);
        }

        public List<EmployeeViewModel> GetEmployeeList()
        {
            List<EmployeeDetailsTbl> employeesList = _employeeService.GetEmployeeList();
            
            return _mapper.Map<List<EmployeeViewModel>>(employeesList);
        }

        public IEnumerable<SelectListItem> GetEmployeeRoleOptions()
        {
            return _employeeService.GetEmployeeRoleOptions()
                           .Select(role => new SelectListItem
                           {
                               Value = role.RoleId.ToString(),
                               Text = role.Role
                           });
        }

        public bool IsDuplicateEmail(string email)
        {
            throw new NotImplementedException();
        }

        public bool IsEmployeeAdded(EmployeeViewModel employee)
        {
            EmployeeDetailsTbl employeeDetails = _mapper.Map<EmployeeDetailsTbl>(employee);
            return _employeeService.IsEmployeeAdded(employeeDetails,employee.Password);
        }

        public bool IsEmployeeDeleted(int empId)
        {
            return _employeeService.IsEmployeeDeleted(empId);
        }

        public bool UpdateEmployeeInfo(EmployeeViewModel employee)
        {
            EmployeeDetailsTbl employeeDetails = _mapper.Map<EmployeeDetailsTbl>(employee);
            return _employeeService.UpdateEmployeeInfo(employeeDetails);
        }
        #endregion 
    }
}
