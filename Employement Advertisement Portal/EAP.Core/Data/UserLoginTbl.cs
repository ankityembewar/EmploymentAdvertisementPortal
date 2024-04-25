using System;
using System.Collections.Generic;

namespace EAP.Core.Data;

public partial class UserLoginTbl
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int ModifiedBy { get; set; }

    public DateTime ModifiedDate { get; set; }

    public int Id { get; set; }
}
