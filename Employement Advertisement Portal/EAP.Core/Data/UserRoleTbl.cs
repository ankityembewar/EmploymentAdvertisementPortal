using System;
using System.Collections.Generic;

namespace EAP.Core.Data;

public partial class UserRoleTbl
{
    public int RoleId { get; set; }

    public string Role { get; set; } = null!;

    public virtual ICollection<EmployeeDetailsTbl> EmployeeDetailsTbls { get; set; } = new List<EmployeeDetailsTbl>();
}
