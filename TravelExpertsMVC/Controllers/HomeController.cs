using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TravelExpertsData;
using TravelExpertsData.Models;
using TravelExpertsMVC.Models;

namespace TravelExpertsMVC.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly TravelExpertssContext _context;

     

        public HomeController(TravelExpertssContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Set the current page's controller and action in ViewData
            ViewData["ActiveController"] = "Home";
            ViewData["ActiveAction"] = "Index";
            ViewBag.Packages = PackageManager.GetPackages(_context);
            return View();
        }

        public IActionResult Privacy()
        {
            // Set the current page's controller and action in ViewData
            ViewData["ActiveController"] = "Home";
            ViewData["ActiveAction"] = "Privacy";
            return View();
        }
        public IActionResult Register()
        {
            // Set the current page's controller and action in ViewData
            ViewData["ActiveController"] = "Home";
            ViewData["ActiveAction"] = "Register";
            return View();
        }

        public IActionResult Bookings()
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
