using AutoMapper;
using EAP.BAL.IAgent.IEmployee;
using EAP.Core.Data;
using EAP.Core.HelperUtilities;
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
        private readonly HelperUtility _helperUtility;
#endregion

        #region Constructor
        public EmployeeAgent(IMapper mapper, IEmployeeService employeeService, HelperUtility helperUtility)
        {
            _mapper = mapper;
            _employeeService = employeeService;
            _helperUtility = helperUtility;
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

        public SMTPViewModel GetSMTPCred(int id)
        {
            SmtpSetting smtpSetting = _helperUtility.GetSMTPCred(id);
            return _mapper.Map<SMTPViewModel>(smtpSetting);
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

        public bool IsSMPTPCredUpdate(SMTPViewModel smtp)
        {
            SmtpSetting smtpSetting = _mapper.Map<SmtpSetting>(smtp);
            return _employeeService.IsSMPTPCredUpdate(smtpSetting);
        }

        public bool UpdateEmployeeInfo(EmployeeViewModel employee)
        {
            EmployeeDetailsTbl employeeDetails = _mapper.Map<EmployeeDetailsTbl>(employee);
            return _employeeService.UpdateEmployeeInfo(employeeDetails);
        }
        #endregion 
    }
}
