using EAP.BAL.IAgent.IAdvertisement;
using EAP.Core.HelperUtilities;
using EAP.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Employement_Advertisement_Portal.Controllers
{
    public class AdvertisementController : Controller
    {
        #region Private Variables
        private readonly HelperUtility _helperUtility;
        private readonly IAdvertisementAgent _advertiseAgent;
        #endregion

        #region Constructor
        public AdvertisementController(IAdvertisementAgent advertiseAgent, HelperUtility helperUtility)
        {
            _advertiseAgent = advertiseAgent;
            _helperUtility = helperUtility;
        }
        #endregion
        [HttpGet]
        public IActionResult List(List<AdvertisementViewModel> advertisements)
        {

            return View(advertisements);

        }

        public IActionResult GetAdvertisements(int page = 0, int pageSize = 9)
        {
            var advertisements = _advertiseAgent.GetAdvertisementList(page, pageSize);
            return PartialView("_AdvertisementsPartial", advertisements);
        }

        [HttpPost]
        public ActionResult Search(string location, string category, int page = 0, int pageSize = 10)
        {
            // Calculate offset based on page number and page size
            int offset = (page - 1) * pageSize;

            // Call search method with pagination parameters
            List<AdvertisementViewModel> advertisements = _advertiseAgent.Search(location, category, offset, pageSize);

            // Return partial view or JSON data
            return PartialView("_AdvertisementsPartial", advertisements);
        }

        public ActionResult Create()
        {
            AdvertisementViewModel advertisement = new AdvertisementViewModel();
            advertisement.AdvertisementCategoryList = _advertiseAgent.GetAdvertisementCategoryOptions();
            return View("CreateEdit", advertisement);
        }

        [HttpPost]
        public ActionResult Create(AdvertisementViewModel advertisement, IFormFile imageFile)
        {
            advertisement.AdvertisementCategoryList = _advertiseAgent.GetAdvertisementCategoryOptions();
            ModelState.Remove("EmployeeDetail");
            //// Check if imageFile is not an image or null and add error message
            if (imageFile == null)
            {
                ModelState["imageFile"].Errors.Clear();
                ModelState["imageFile"].Errors.Add("This field is required.");
            }
            else
            {
                var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState["imageFile"].Errors.Clear();
                    ModelState["imageFile"].Errors.Add("Please upload a valid image file.");
                }
            }
            if(ModelState["AdvCategoryId"].Errors.Count > 0)
            {
                ModelState["AdvCategoryId"].Errors.Clear();
                ModelState["AdvCategoryId"].Errors.Add("This field is required.");
            }
            if (ModelState.IsValid)
            {
                advertisement.MediaPath = _helperUtility.SaveImage(imageFile);
                _advertiseAgent.IsAdvertisementCreated(advertisement);
                return RedirectToAction("List");
            }

            return View("CreateEdit", advertisement);
        }


        public ActionResult Edit(int advId)
        {
            AdvertisementViewModel advertisement = _advertiseAgent.GetAdvertisementInfo(advId);
            advertisement.AdvertisementCategoryList = _advertiseAgent.GetAdvertisementCategoryOptions();
            return PartialView("CreateEdit", advertisement);
        }

        [HttpPost]
        public ActionResult Edit(AdvertisementViewModel advertisement, IFormFile imageFile)
        {
            ModelState.Remove("EmployeeDetail");
            ModelState.Remove("imageFile");
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    advertisement.MediaPath = _helperUtility.SaveImage(imageFile);
                }

                _advertiseAgent.IsAdvertisementEdit(advertisement);
                return RedirectToAction("UserAdvertisementList");
            }
            return RedirectToAction("Edit", new { advId = advertisement.AdvId });


        }

        [HttpPost]
        public ActionResult Delete(int advId)
        {
            try
            {
                bool result = _advertiseAgent.IsAdvertisementDeleted(advId);
                return Json(new { success = result });
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("Index");
            }
        }

        public IActionResult UserAdvertisementList()
        {
            return View();
        }

        public IActionResult GetUserAdvertisementList(int empId)
        {
            List<AdvertisementViewModel> advertisements = _advertiseAgent.UserAdvertisementList(empId);
            return new JsonResult(advertisements);
        }


    }
}
