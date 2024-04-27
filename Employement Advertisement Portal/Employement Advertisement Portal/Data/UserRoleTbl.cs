using System;
using System.Collections.Generic;

namespace Employement_Advertisement_Portal.Data;

public partial class UserRoleTbl
{
    public int RoleId { get; set; }

    public string Role { get; set; } = null!;

    public virtual ICollection<EmployeeDetailsTbl> EmployeeDetailsTbls { get; set; } = new List<EmployeeDetailsTbl>();
}
