using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TravelExpertsData;
using TravelExpertsData.Models;
using TravelExpertsMVC.Models;
using TravelExpertsData.Models;
using TravelExpertsData.ViewModels;

namespace TravelExpertsMVC.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly TravelExpertsContext _context;

     

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, TravelExpertsContext context)
        {
            _context = context;
            this._context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Set the current page's controller and action in ViewData
            ViewData["ActiveController"] = "Home";
            ViewData["ActiveAction"] = "Index";
            ViewBag.Packages = PackageManager.GetPackages(_context);
            if (User.Identity.IsAuthenticated)
            {
                Customer customerDetails = getCurrentUser();
                if (customerDetails != null)
                {
                    ViewData["FirstName"] = customerDetails.CustFirstName;
                    ViewData["ProfilePicture"]=customerDetails.ProfileImagePath;
                }

            }

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
        public IActionResult ContactUs()
        {
            // Use ContactManager to fetch data
            var agenciesWithAgents = ContactManager.GetAgenciesWithAgents(_context);
            // Pass the data to the view
            return View(agenciesWithAgents);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public Customer getCurrentUser()
        {
            string userName = User.Identity.Name;

            var customer = _context.Customers.FirstOrDefault(cus => cus.CustEmail == userName);
            return customer;
        }
    }
}
