using EAP.Core.Data;
using EAP.Core.HelperUtilities;
using EAP.DAL.IService.IAdvertisement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAP.DAL.Service.Advertisement
{
    public class AdvertisementService : IAdvertisementService
    {
        #region Private Variables
        private readonly HelperUtility _helperUtility;
        #endregion

        #region Constructor
        public AdvertisementService(HelperUtility helperUtility)
        {
            _helperUtility = helperUtility;
        }
        #endregion
        public List<AdvertisementDetailsTbl> GetAdvertisementList()
        {
            using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
            {
                return context.AdvertisementDetailsTbls.Include(x => x.AdvCategory).Include(x=>x.Emp).ToList();
            }
        }

        public bool IsAdvertisementCreated(AdvertisementDetailsTbl advertisement)
        {
            using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
            {
                if (advertisement.EmpId <= 0)
                {
                    advertisement.EmpId = _helperUtility.GetEmployeeId();
                }
                _helperUtility.MapAuditFields(advertisement, "CreatedBy", "ModifiedBy", "CreatedDate", "ModifiedDate", false);
                context.AdvertisementDetailsTbls.Add(advertisement);
                context.SaveChanges();
                return true;
            }
        }

        public List<AdvertisementCategoryTbl> GetAdvertisementCategoryOptions()
        {
            using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
            {
                return context.AdvertisementCategoryTbls.ToList();
            }
        }

    }
}
