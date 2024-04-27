using EAP.Core.Data;
using EAP.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
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
        public List<AdvertisementViewModel> GetAdvertisementList();

        public bool IsAdvertisementCreated(AdvertisementViewModel advertisement);

        public IEnumerable<SelectListItem> GetAdvertisementCategoryOptions();
    }
}
