using AutoMapper;
using EAP.BAL.IAgent.IEmployee;
using EAP.Core.Data;
using EAP.DAL.IService.Employee;
using EAP.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            throw new NotImplementedException();
        }

        public List<EmployeeViewModel> GetEmployeeList()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SelectListItem> GetEmployeeRoleOptions()
        {
            throw new NotImplementedException();
        }

        public bool IsDuplicateEmail(string email)
        {
            throw new NotImplementedException();
        }

        public bool IsEmployeeAdded(EmployeeViewModel employee)
        {
            throw new NotImplementedException();
        }

        public bool IsEmployeeDeleted(int empId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateEmployeeInfo(EmployeeViewModel employee)
        {
            throw new NotImplementedException();
        }
        #endregion 
    }
}
