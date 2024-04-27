using AutoMapper;
using EAP.BAL.IAgent.IAdvertisement;
using EAP.Core.Data;
using EAP.DAL.IService.Employee;
using EAP.DAL.IService.IAdvertisement;
using EAP.DAL.Service.Employee;
using EAP.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAP.BAL.Agent.Advertisement
{
    public class AdvertisementAgent : IAdvertisementAgent
    {
        #region Private Variables
        private readonly IMapper _mapper;
        private readonly IAdvertisementService _advertiseService;
        #endregion

        #region Constructor
        public AdvertisementAgent(IMapper mapper, IAdvertisementService advertiseService)
        {
            _mapper = mapper;
            _advertiseService = advertiseService;
        }
        #endregion

        #region Method
        public List<AdvertisementViewModel> GetAdvertisementList()
        {
            List<AdvertisementDetailsTbl> advertisementList= _advertiseService.GetAdvertisementList();
            return _mapper.Map<List<AdvertisementViewModel>>(advertisementList);
        }

        public bool IsAdvertisementCreated(AdvertisementViewModel advertisement)
        {
            AdvertisementDetailsTbl advertisementDetails = _mapper.Map<AdvertisementDetailsTbl>(advertisement);
            return _advertiseService.IsAdvertisementCreated(advertisementDetails);
        }

        public IEnumerable<SelectListItem> GetAdvertisementCategoryOptions()
        {
            return _advertiseService.GetAdvertisementCategoryOptions()
                           .Select(role => new SelectListItem
                           {
                               Value = role.AdvCategoryId.ToString(),
                               Text = role.Category
                           });
        }

        #endregion
    }
}
