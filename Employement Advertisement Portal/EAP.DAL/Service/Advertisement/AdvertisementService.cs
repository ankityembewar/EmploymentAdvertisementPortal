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
                return context.AdvertisementDetailsTbls.Include(x => x.AdvCategory).Include(x=>x.Emp).Where(x=>x.IsApproved==true).ToList();
            }
        }

        public List<AdvertisementDetailsTbl> GetAdvertisementRequestList()
        {
            using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
            {
                return context.AdvertisementDetailsTbls.Include(x => x.AdvCategory).Include(x => x.Emp).ToList();
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

        public AdvertisementDetailsTbl GetDetailAdvertisementById(int advId)
        {
            using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
            {
                return context.AdvertisementDetailsTbls.Include(x => x.AdvCategory).Include(x => x.Emp).First(x => x.AdvId == advId);
            }
        }

        public bool ActionOnAdvertisement(int advId, string decision)
        {
            using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
            {
                AdvertisementDetailsTbl advertisementDetails = GetDetailAdvertisementById(advId);
                if (advertisementDetails != null)
                {
                    if(decision== "approved")
                    {
                        advertisementDetails.IsApproved = true;
                        advertisementDetails.IsRejected = false;
                    }
                    else if(decision== "rejected")
                    {
                        advertisementDetails.IsRejected = true;
                        advertisementDetails.IsApproved = false;
                    }
                    _helperUtility.MapAuditFields(advertisementDetails, "CreatedBy", "ModifiedBy", "CreatedDate", "ModifiedDate", true);
                    context.Entry(advertisementDetails).State = EntityState.Modified;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool IsAdvertisementDeleted(int advId)
        {
            using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
            {
                // Retrieve the employee from the database
                AdvertisementDetailsTbl advertiseToDelete = context.AdvertisementDetailsTbls.FirstOrDefault(x => x.AdvId == advId);

                if (advertiseToDelete != null)
                {
                    // Remove related records from AdvertisementDetails_tbl
                    //var relatedAds = context.AdvertisementDetailsTbls.Where(ad => ad.EmpId == empId);
                    //context.AdvertisementDetailsTbls.RemoveRange(relatedAds);

                    // Remove the employee from the context
                    context.AdvertisementDetailsTbls.Remove(advertiseToDelete);

                    // Save changes to persist the deletion
                    context.SaveChanges();

                    return true; // Employee successfully deleted
                }

                return false; // Employee not found or deletion failed
            }
        }

        public List<AdvertisementDetailsTbl> UserAdvertisementList(int userId)
        {
            using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
            {
                return context.AdvertisementDetailsTbls.Include(x => x.AdvCategory).Include(x => x.Emp).Where(x => x.EmpId == userId).ToList();
            }
        }

        public bool IsAdvertisementEdit(AdvertisementDetailsTbl advertisement)
        {
            try
            {
                using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
                {
                    _helperUtility.MapAuditFields(advertisement, "CreatedBy", "ModifiedBy", "CreatedDate", "ModifiedDate", true);
                    context.Entry(advertisement).State = EntityState.Modified;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public AdvertisementDetailsTbl GetAdvertisementInfo(int advId)
        {
            using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
            {
                return context.AdvertisementDetailsTbls.Include(x => x.AdvCategory).Include(x => x.Emp).Where(x => x.AdvId == advId).FirstOrDefault();
            }
        }
    }
}
