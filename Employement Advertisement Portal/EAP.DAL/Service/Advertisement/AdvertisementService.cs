using EAP.Core.Data;
using EAP.Core.HelperUtilities;
using EAP.DAL.IService.IAdvertisement;
using Microsoft.EntityFrameworkCore;
using EAP.Core.ResourceFile;
using Azure;
using System.Drawing.Printing;

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
        public List<AdvertisementDetailsTbl> GetAdvertisementList(int page = 0, int pageSize = 9)
        {
            using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
            {
                // Ensure page is not less than 0
                if (page < 0)
                {
                    page = 0;
                }
                int skipCount = page * pageSize;
                List<AdvertisementDetailsTbl> advertisementDetails = new List<AdvertisementDetailsTbl>();
                return context.AdvertisementDetailsTbls
                    .Include(x => x.AdvCategory)
                    .Include(x => x.Emp)
                    .Where(x => x.IsApproved && x.EmpId != _helperUtility.GetEmployeeId())
                    .OrderByDescending(x => x.CreatedBy) // Example sorting
                    .Skip(skipCount)
                    .Take(pageSize)
                    .ToList();
                
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

                    string subject = "Action On Created Advertisement";
                    string approvalStatusMessage = advertisementDetails.IsApproved ? "approved" : "rejected";


                    string body = EmailTemplate.ActionOnAdvertisement;
                    body = body.Replace("{employeeFirstName}", advertisementDetails.Emp.FirstName)
           .Replace("{approvalStatusMessage}", approvalStatusMessage)
           .Replace("{adTitle}", advertisementDetails.Title)
           .Replace("{adDescription}", advertisementDetails.Description)
           .Replace("{createdDate}", advertisementDetails.CreatedDate.ToString())
           .Replace("{actionDate}", advertisementDetails.ModifiedDate.ToString());
                    _helperUtility.SendEmail(advertisementDetails.Emp.Email, "ankit@yopmail.com", subject, body);

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

        public List<AdvertisementDetailsTbl> Search(string location, string category,int offset, int pageSize)
        {
            using (EmployeeAdvertisementPortalContext context = new EmployeeAdvertisementPortalContext())
            {
                if(location!=null && category != null)
                {
                    return context.AdvertisementDetailsTbls.Include(x => x.AdvCategory).Include(x => x.Emp).Where(x => x.AdvCategory.Category.ToLower() == category.ToLower() && x.Location.ToLower() == location.ToLower()).Skip(offset).Take(pageSize).ToList();
                }
                else if (location != null)
                {
                    return context.AdvertisementDetailsTbls.Include(x => x.AdvCategory).Include(x => x.Emp).Where(x => x.Location.ToLower() == location.ToLower()).Skip(offset).Take(pageSize).ToList();
                }
                else if (category != null)
                {
                    return context.AdvertisementDetailsTbls.Include(x => x.AdvCategory).Include(x => x.Emp).Where(x => x.AdvCategory.Category.ToLower() == category.ToLower()).Skip(offset).Take(pageSize).ToList();
                }
                else
                {
                    return context.AdvertisementDetailsTbls
                    .Include(x => x.AdvCategory)
                    .Include(x => x.Emp)
                    .Where(x => x.IsApproved)
                    .OrderByDescending(x => x.AdvId) // Example sorting
                    .Skip(offset)
                    .Take(pageSize)
                    .ToList();
                }

            }
        }
    }
}
