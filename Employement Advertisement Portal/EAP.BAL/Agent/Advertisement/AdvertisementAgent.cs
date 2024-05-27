using AutoMapper;
using EAP.BAL.IAgent.IAdvertisement;
using EAP.Core.Data;
using EAP.Core.HelperUtilities;
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
        public AdvertisementAgent(IMapper mapper, IAdvertisementService advertiseService, HelperUtility helperUtility)
        {
            _mapper = mapper;
            _advertiseService = advertiseService;
        }
        #endregion

        #region Method
        public List<AdvertisementViewModel> GetAdvertisementList(int page = 1, int pageSize = 3)
        {
            List<AdvertisementDetailsTbl> advertisementList= _advertiseService.GetAdvertisementList(page,pageSize);
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

        public List<AdvertisementViewModel> GetAdvertisementRequestList()
        {
            List<AdvertisementDetailsTbl> advertisementList = _advertiseService.GetAdvertisementRequestList();
            return _mapper.Map<List<AdvertisementViewModel>>(advertisementList);
        }

        public bool ActionOnAdvertisement(int advId, string decision)
        {
            if (advId != 0 && decision != null)
                return _advertiseService.ActionOnAdvertisement(advId, decision);
            else
                return false;
        }

        public bool IsAdvertisementDeleted(int advId)
        {
            return _advertiseService.IsAdvertisementDeleted(advId);
        }

        public List<AdvertisementViewModel> UserAdvertisementList(int userId)
        {
            List<AdvertisementDetailsTbl> advertisementList = _advertiseService.UserAdvertisementList(userId);
            return _mapper.Map<List<AdvertisementViewModel>>(advertisementList);
        }

        public bool IsAdvertisementEdit(AdvertisementViewModel advertisement)
        {
            AdvertisementDetailsTbl advertisementDetails = _mapper.Map<AdvertisementDetailsTbl>(advertisement);
            return _advertiseService.IsAdvertisementEdit(advertisementDetails);
        }

        public AdvertisementViewModel GetAdvertisementInfo(int advId)
        {
            AdvertisementDetailsTbl advertisement = _advertiseService.GetAdvertisementInfo(advId);
            return _mapper.Map<AdvertisementViewModel>(advertisement);
        }

        public List<AdvertisementViewModel> Search(string location, string category, int offset, int pageSize)
        {
            List<AdvertisementDetailsTbl> advertisementList = _advertiseService.Search(location, category,offset,pageSize);
            return _mapper.Map<List<AdvertisementViewModel>>(advertisementList);
        }



        #endregion
    }
}
