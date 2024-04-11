using System;
using System.Collections.Generic;

namespace EAP.Core.Data;

public partial class AdvertisementCategoryTbl
{
    public int AdvCategoryId { get; set; }

    public string Category { get; set; } = null!;

    public virtual ICollection<AdvertisementDetailsTbl> AdvertisementDetailsTbls { get; set; } = new List<AdvertisementDetailsTbl>();
}
