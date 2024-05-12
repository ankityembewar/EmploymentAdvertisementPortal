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
        public IActionResult List()
        {
            List<AdvertisementViewModel> advertisements = _advertiseAgent.GetAdvertisementList();
            return View(advertisements);

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
            advertisement.MediaPath = _helperUtility.SaveImage(imageFile);
            _advertiseAgent.IsAdvertisementCreated(advertisement);
            return RedirectToAction("List");
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
            advertisement.MediaPath = _helperUtility.SaveImage(imageFile);
            _advertiseAgent.IsAdvertisementEdit(advertisement);
            return RedirectToAction("UserAdvertisementList");
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

        [HttpPost]
        public ActionResult Search(string location, string category)
        {
            List<AdvertisementViewModel> advertisements = _advertiseAgent.Search(location, category);
            return View("List", advertisements);
        }

    }
}
