using EAP.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Employement_Advertisement_Portal.Controllers
{
    public class AdvertisementController : Controller
    {
        public IActionResult List()
        {
            // Create sample data
            var advertisements = new List<AdvertisementViewModel>
        {
            new AdvertisementViewModel
            {
                AdvId = 1,
                Title = "Sample Advertisement 1",
                Description = "Description for sample advertisement 1",
                Price = 100,
                Location = "Sample Location 1",
                MediaPath = "/images/advertisement1.jpg" // Provide the path to the image
                // Add other properties as needed
            },
            new AdvertisementViewModel
            {
                AdvId = 2,
                Title = "Sample Advertisement 2",
                Description = "Description for sample advertisement 2",
                Price = 200,
                Location = "Sample Location 2",
                MediaPath = "/images/advertisement2.jpg" // Provide the path to the image
                // Add other properties as needed
            },
            // Add more sample data as needed
        };

            return View(advertisements);
        }
    }
}
