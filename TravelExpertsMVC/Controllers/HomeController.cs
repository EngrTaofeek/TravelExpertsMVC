using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TravelExpertsData;
using TravelExpertsData.Models;
using TravelExpertsMVC.Models;
using TravelExpertsData.Models;

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
            ViewBag.Packages = PackageManager.GetPackages(_context);
            if (User.Identity.IsAuthenticated)
            {
                Customer customerDetails = getCurrentUser();
                if (customerDetails != null)
                {
                    ViewData["FirstName"] = customerDetails.CustFirstName;
                }

            }

            return View();
        }

        public IActionResult Privacy()
        {
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

        public Customer getCurrentUser()
        {
            string userName = User.Identity.Name;

            var customer = _context.Customers.FirstOrDefault(cus => cus.CustEmail == userName);
            return customer;
        }
    }
}
