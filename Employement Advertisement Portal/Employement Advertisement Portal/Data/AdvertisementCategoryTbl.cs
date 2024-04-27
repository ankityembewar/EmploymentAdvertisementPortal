using System;
using System.Collections.Generic;

namespace Employement_Advertisement_Portal.Data;

public partial class AdvertisementCategoryTbl
{
    public int AdvCategoryId { get; set; }

    public string Category { get; set; } = null!;

    public virtual ICollection<AdvertisementDetailsTbl> AdvertisementDetailsTbls { get; set; } = new List<AdvertisementDetailsTbl>();
}
