using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAP.ViewModel
{
    public class EmployeeViewModel
    {
        public int EmpId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Gender { get; set; } = null!;

        public DateTime Dob { get; set; }

        public string Address { get; set; } = null!;

        public int RoleId { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public IEnumerable<SelectListItem> EmployeeRole { get; set; }
    }
}
