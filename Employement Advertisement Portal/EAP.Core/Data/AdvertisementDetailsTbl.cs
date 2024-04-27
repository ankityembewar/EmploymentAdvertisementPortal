using System;
using System.Collections.Generic;

namespace EAP.Core.Data;

public partial class AdvertisementDetailsTbl
{
    public int AdvId { get; set; }

    public int EmpId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Price { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int ModifiedBy { get; set; }

    public DateTime ModifiedDate { get; set; }

    public string Location { get; set; } = null!;

    public int AdvCategoryId { get; set; }

    public bool IsApproved { get; set; }

    public bool IsRejected { get; set; }

    public string MediaPath { get; set; } = null!;

    public virtual AdvertisementCategoryTbl AdvCategory { get; set; } = null!;

    public virtual EmployeeDetailsTbl Emp { get; set; } = null!;
}
