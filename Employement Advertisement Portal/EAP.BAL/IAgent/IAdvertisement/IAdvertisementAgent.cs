using EAP.Core.Data;
using EAP.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAP.BAL.IAgent.IAdvertisement
{
    public interface IAdvertisementAgent
    {
        /// <summary>
        /// Retrieves a list of advertisement.
        /// </summary>
        List<AdvertisementViewModel> GetAdvertisementList(int page = 0, int pageSize = 9);
        public bool IsAdvertisementCreated(AdvertisementViewModel advertisement);

        public IEnumerable<SelectListItem> GetAdvertisementCategoryOptions();

        public List<AdvertisementViewModel> GetAdvertisementRequestList();

        bool ActionOnAdvertisement(int advId, string decision);

        bool IsAdvertisementDeleted(int advId);

        public List<AdvertisementViewModel> UserAdvertisementList(int userId);

        public bool IsAdvertisementEdit(AdvertisementViewModel advertisement);

        public AdvertisementViewModel GetAdvertisementInfo(int advId);

        public List<AdvertisementViewModel> Search(string location, string category, int offset, int pageSize);

    }
}
