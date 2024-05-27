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
        List<AdvertisementDetailsTbl> GetAdvertisementList(int page = 0, int pageSize = 9);

        public bool IsAdvertisementCreated(AdvertisementDetailsTbl advertisement);

        public List<AdvertisementCategoryTbl> GetAdvertisementCategoryOptions();
        public List<AdvertisementDetailsTbl> GetAdvertisementRequestList();

        public AdvertisementDetailsTbl GetDetailAdvertisementById(int advId);

        bool ActionOnAdvertisement(int advId, string decision);

        bool IsAdvertisementDeleted(int advId);

        public List<AdvertisementDetailsTbl> UserAdvertisementList(int userId);

        public bool IsAdvertisementEdit(AdvertisementDetailsTbl advertisement);
        public AdvertisementDetailsTbl GetAdvertisementInfo(int advId);
        public List<AdvertisementDetailsTbl> Search(string location, string category, int offset, int pageSize);
    }
}
