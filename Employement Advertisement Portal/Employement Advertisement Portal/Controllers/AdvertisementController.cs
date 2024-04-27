using EAP.BAL.IAgent.IAdvertisement;
using EAP.BAL.IAgent.IEmployee;
using EAP.Core.HelperUtilities;
using EAP.ViewModel;
using Microsoft.AspNetCore.Mvc;

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

    }
}
