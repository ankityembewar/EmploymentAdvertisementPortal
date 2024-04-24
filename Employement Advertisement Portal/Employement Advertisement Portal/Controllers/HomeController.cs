﻿using EAP.Core.HelperUtilities;
using Employement_Advertisement_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Employement_Advertisement_Portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HelperUtility _helperUtility;

        public HomeController(ILogger<HomeController> logger, HelperUtility helperUtility)
        {
            _logger = logger;
            _helperUtility = helperUtility;
        }

        public IActionResult Index()
        {
            
            if (_helperUtility.IsUserLoggedIn())
            {
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("UserLogin","Login");


        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
