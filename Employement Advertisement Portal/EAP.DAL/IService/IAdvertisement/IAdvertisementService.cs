using EAP.Core.Data;
using EAP.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAP.DAL.IService.IAdvertisement
{
    public interface IAdvertisementService
    {
        /// <summary>
        /// Retrieves a list of advertisement.
        /// </summary>
        public List<AdvertisementDetailsTbl> GetAdvertisementList();

        public bool IsAdvertisementCreated(AdvertisementDetailsTbl advertisement);

        public List<AdvertisementCategoryTbl> GetAdvertisementCategoryOptions();
    }
}
