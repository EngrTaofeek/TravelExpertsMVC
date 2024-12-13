using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TravelExpertsData;
using TravelExpertsData.Models;
using TravelExpertsMVC.Models;
using TravelExpertsData.Models;
using TravelExpertsData.ViewModels;
using System.Net.Mail;
using System.Net;

namespace TravelExpertsMVC.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly TravelExpertsContext _context;

     

        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;


        public HomeController(ILogger<HomeController> logger, TravelExpertsContext context, IConfiguration configuration)
        {
            _context = context;
            this._context = context;
            _configuration = configuration;
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
            if (User.Identity.IsAuthenticated)
            {
                Customer customerDetails = getCurrentUser();
                if (customerDetails != null)
                {
                    ViewData["FirstName"] = customerDetails.CustFirstName;
                    ViewData["ProfilePicture"] = customerDetails.ProfileImagePath;
                }

            }
            return View(agenciesWithAgents);
        }
        [HttpPost]
        public IActionResult ContactUs(string customerEmail, string feedbackText)
        {
            if (ModelState.IsValid)
            {
                // Call your existing email-sending method
                SendFeedbackEmail(customerEmail, feedbackText);

                // Optionally, display a success message or redirect
                TempData["SuccessMessage"] = "Thank you for your feedback!";
                return RedirectToAction("ContactUs"); // Or another action
            }

            // If something went wrong, re-display the form
            return View();
        }
        // Method to send a confirmation email
        private void SendFeedbackEmail(string customerEmail, string feedbackText)
        {
            try
            {
                // Retrieve email configuration from appsettings.json
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var port = int.Parse(_configuration["EmailSettings:Port"]);
                var senderEmail = _configuration["EmailSettings:SenderEmail"];
                var senderPassword = _configuration["EmailSettings:SenderPassword"];

                using (var smtpClient = new SmtpClient(smtpServer, port))
                {
                    // Configure SMTP client
                    smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    smtpClient.EnableSsl = true;


                    // Define the email subject and body
                    var subject = "CUSTOMER FEEDBACK";
                    var body = $@"
                            From {customerEmail},
                            {feedbackText}
                        ";

                    // Create and send the email
                    var mailMessage = new MailMessage(senderEmail, customerEmail, subject, body);
                    smtpClient.Send(mailMessage);

                    Console.WriteLine("Feedback email sent successfully.");

                }
            }
            catch (Exception ex)
            {
                // Log any errors that occur during the email-sending process
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
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
