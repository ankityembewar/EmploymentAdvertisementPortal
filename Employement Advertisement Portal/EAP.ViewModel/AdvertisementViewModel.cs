using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAP.ViewModel
{
    public class AdvertisementViewModel
    {
        public int AdvId { get; set; }

        public int EmpId { get; set; }

        [StringLength(80, ErrorMessage = "The {0} must be at most {1} characters long.")]
        [Required(ErrorMessage = "This field is required.")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "The {0} must be at most {1} characters long.")]
        [Required(ErrorMessage = "This field is required.")]
        public string Description { get; set; } = null!;
       
        [Required(ErrorMessage = "This field is required.")]
        public int Price { get; set; }

        public int PostedBy { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string Location { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        public int AdvCategoryId { get; set; }

        public bool IsApproved { get; set; }

        public bool IsRejected { get; set; }

        public string? MediaPath { get; set; }

        public IEnumerable<SelectListItem> AdvertisementCategoryList { get; set; }

        public EmployeeViewModel EmployeeDetail{ get; set; }
    }
}
