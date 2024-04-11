using System;
using System.Collections.Generic;

namespace EAP.Core.Data;

public partial class UserLoginTbl
{
    public int EmpId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int CreatedBy { get; set; }

    public string CreatedDate { get; set; } = null!;

    public int ModifiedBy { get; set; }

    public DateTime ModifiedDate { get; set; }

    public virtual EmployeeDetailsTbl Emp { get; set; } = null!;
}
