using EAP.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAP.BAL.IAgent.IEmployee
{
    public interface IEmployeeAgent
    {
        /// <summary>
        /// Retrieves a list of employee view models.
        /// </summary>
        /// <returns>A list containing view models representing all employees in the system.</returns>
        List<EmployeeViewModel> GetEmployeeList();

        /// <summary>
        /// Checks if the provided employee has been successfully added to the system.
        /// </summary>
        /// <param name="employee">The employee view model to check.</param>
        /// <returns>True if the employee has been successfully added, otherwise false.</returns>
        bool IsEmployeeAdded(EmployeeViewModel employee);

        /// <summary>
        /// Fetches detailed information about an employee based on their unique employee ID.
        /// </summary>
        /// <param name="empId">The unique ID of the employee.</param>
        /// <returns>The view model representing the employee's information.</returns>
        EmployeeViewModel GetEmployeeInfo(int empId);

        /// <summary>
        /// Updates the information of an existing employee with the details provided in the EmployeeViewModel.
        /// </summary>
        /// <param name="employee">The updated employee view model.</param>
        /// <returns>True if the employee information was successfully updated, otherwise false.</returns>
        bool UpdateEmployeeInfo(EmployeeViewModel employee);

        /// <summary>
        /// Verifies whether the employee with the specified ID has been successfully deleted from the system.
        /// </summary>
        /// <param name="empId">The unique ID of the employee to check.</param>
        /// <returns>True if the employee has been successfully deleted, otherwise false.</returns>
        bool IsEmployeeDeleted(int empId);

        /// <summary>
        /// Retrieves employee information based on their email address.
        /// </summary>
        /// <param name="email">The email address of the employee to retrieve.</param>
        /// <returns>The view model representing the employee's information.</returns>
        EmployeeViewModel GetEmployeeByEmail(string email);

        /// <summary>
        /// Checks whether the provided email address already exists in the system for another employee.
        /// </summary>
        /// <param name="email">The email address to check for duplicates.</param>
        /// <returns>True if the email address is a duplicate, otherwise false.</returns>
        bool IsDuplicateEmail(string email);

        /// <summary>
        /// Retrieves a collection of SelectListItem representing the various role options available for employees in the system.
        /// </summary>
        /// <returns>A collection of SelectListItem representing employee role options.</returns>
        IEnumerable<SelectListItem> GetEmployeeRoleOptions();

        /// <summary>
        /// Retrieves SMTP credentials for a specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the SMTP credentials to retrieve.</param>
        /// <returns>An SMTPViewModel object containing the SMTP credentials for the specified identifier.</returns>
        SMTPViewModel GetSMTPCred(int id);

        /// <summary>
        /// Determines if the provided SMTP credentials need updating in the system.
        /// </summary>
        /// <param name="smtp">The SMTPViewModel instance containing the SMTP credentials to verify.</param>
        /// <returns>True if the SMTP credentials should be updated, otherwise false.</returns>
        bool IsSMPTPCredUpdate(SMTPViewModel smtp);
    }
}
