using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelExpertsData;
using TravelExpertsData.Models;
using TravelExpertsData.ViewModels;
using iText.Kernel.Pdf; // For PDF document creation
using iText.Layout; // For layout and document creation
using iText.Layout.Element; // For adding elements like Paragraph, Image, etc.
using iText.IO.Image; // For handling images in the PDF
using System.IO; // For MemoryStream


namespace TravelExpertsMVC.Controllers
{
    public class BookingController : Controller
    {
        private readonly TravelExpertsContext _context; 
        private readonly IConfiguration _configuration;


        public BookingController(TravelExpertsContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Booking
        public async Task<IActionResult> Index()
        {
            Customer customerDetails = getCurrentUser();
            if (User.Identity.IsAuthenticated)
            {
                
                if (customerDetails != null)
                {
                    ViewData["FirstName"] = customerDetails.CustFirstName;
                    ViewData["ProfilePicture"] = customerDetails.ProfileImagePath;
                }

            }
            // Set the current page's controller and action in ViewData
            ViewData["ActiveController"] = "Booking";
            ViewData["ActiveAction"] = "Index";
            var travelExpertssContext = _context.Bookings.Include(b => b.Customer).Include(b => b.Package).Include(b => b.TripType);
            ViewBag.Packages = PackageManager.GetPackages(_context);
            //should update customer Id to that of signed in customer
            List<BookingViewModel> bookings = BookingManager.GetBookings(_context, customerDetails.CustomerId);
            return View(bookings);
        }

        // GET: Booking/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Set the current page's controller and action in ViewData
            ViewData["ActiveController"] = "Booking";
            ViewData["ActiveAction"] = "Details";
            if (id == null)
            {
                return NotFound();
            }
            if (User.Identity.IsAuthenticated)
            {
                Customer customerDetails = getCurrentUser();
                if (customerDetails != null)
                {
                    ViewData["FirstName"] = customerDetails.CustFirstName;
                    ViewData["ProfilePicture"] = customerDetails.ProfileImagePath;
                }

            }

            BookingViewModel booking = BookingManager.GetBookingById(_context, (int)id);

            return View(booking);
        }

        // GET: Booking/Create
        public IActionResult Create(int? packageId)
        {
            Customer customerDetails = getCurrentUser();
            // Set the current page's controller and action in ViewData
            ViewData["ActiveController"] = "Booking";
            ViewData["ActiveAction"] = "Create";
            if (User.Identity.IsAuthenticated)
            {
                
                if (customerDetails != null)
                {
                    ViewData["FirstName"] = customerDetails.CustFirstName;
                    ViewData["ProfilePicture"] = customerDetails.ProfileImagePath;
                }

            }
            PackageViewModel package = PackageManager.GetPackageById(_context,(int) packageId);
            //hardcoded customerId
            List<CreditCardViewModel> creditCards = CreditCardManager.GetCreditCardsForCustomer(_context, customerDetails.CustomerId);
            List<TripType> tripTypes = TripManager.GetTripTypes(_context);
            PrebookingCreationViewModel prebookingCreationViewModel = new PrebookingCreationViewModel
            {
                Package = package,
                CreditCards = creditCards,
                TripTypes = tripTypes,
            };
            return View(prebookingCreationViewModel);
        }

        // POST: Booking/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("BookingId,BookingDate,BookingNo,TravelerCount,CustomerId,TripTypeId,PackageId,PaymentStatus,Balance,TotalPaid")] Booking booking)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(booking);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustAddress", booking.CustomerId);
        //    ViewData["PackageId"] = new SelectList(_context.Packages, "PackageId", "PkgName", booking.PackageId);
        //    ViewData["TripTypeId"] = new SelectList(_context.TripTypes, "TripTypeId", "TripTypeId", booking.TripTypeId);
        //    return View(booking);
        //}

        [HttpPost] 
        public IActionResult Create(BookingViewModel booking)
        {
            // Check if a credit card is selected
            if (booking.SelectedCreditCardId == null)
            {
                TempData["ErrorMessage"] = "Please select a credit card.";
                return RedirectToAction("Create", new { packageId = booking.Package.Id });
            }

            var selectedCreditCardId = booking.SelectedCreditCardId;
            var packageTotalPrice = booking.Package.TotalPrice;
            Customer customerDetails = getCurrentUser();

            // Attempt to deduct balance and update credit card
            bool isBalanceSufficient = CreditCardManager.DeductAmountAndUpdateBalance(_context, selectedCreditCardId, packageTotalPrice);

            if (!isBalanceSufficient)
            {
                // If insufficient funds, return with an error message
                TempData["ErrorMessage"] = "Insufficient funds on the selected credit card.";
                return RedirectToAction("Create", new { packageId = booking.Package.Id });
            }

            var bookingNumber = GenerateBookingNumber();
            // Proceed to save booking in the database
            var newBooking = new Booking
            {
                CustomerId = customerDetails.CustomerId,
                BookingDate = DateTime.Now,
                BookingNo = bookingNumber,
                TripTypeId = booking.TripTypeId,
                TravelerCount = booking.TravelersCount,
                PaymentStatus = "Paid",
                Balance = 0,
                TotalPaid = packageTotalPrice,
                PackageId = booking.Package.Id
            };

            _context.Bookings.Add(newBooking);
            _context.SaveChanges();
            SendConfirmationEmail(booking);

            return RedirectToAction("ThankYou", new { bookingNumber });
        }

        public IActionResult ThankYou(string bookingNumber)
        {
            if (User.Identity.IsAuthenticated)
            {
                Customer customerDetails = getCurrentUser();
                if (customerDetails != null)
                {
                    ViewData["FirstName"] = customerDetails.CustFirstName;
                    ViewData["ProfilePicture"] = customerDetails.ProfileImagePath;
                }
            }

            var booking = _context.Bookings.Include(b => b.Package).FirstOrDefault(b => b.BookingNo == bookingNumber);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        public IActionResult GeneratePdf(string bookingNumber)
        {
            var booking = _context.Bookings.Include(b => b.Package).FirstOrDefault(b => b.BookingNo == bookingNumber);

            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            using (var ms = new MemoryStream())
            {
                var writer = new PdfWriter(ms);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                // Add logo
                var logo = ImageDataFactory.Create("wwwroot/images/logo.png");
                var img = new iText.Layout.Element.Image(logo).SetWidth(100);
                document.Add(img);

                // Add title
                var title = new Paragraph("Travel Experts Booking Confirmation")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFontSize(18);
                document.Add(title);

                // Add booking details

                document.Add(new Paragraph($"Customer Name: {booking.BookingNo}"));
                document.Add(new Paragraph($"Booking Number: {booking.BookingNo}"));
                document.Add(new Paragraph($"Booking Date: {booking.BookingDate?.ToString("yyyy-MM-dd")}"));
                document.Add(new Paragraph($"Package Name: {booking.Package.PkgName}"));
                document.Add(new Paragraph($"Traveler Count: {booking.TravelerCount}"));
                document.Add(new Paragraph($"Total Paid: {booking.TotalPaid.ToString("C")}"));
                document.Add(new Paragraph($"Payment Status: {booking.PaymentStatus}"));

                document.Close();

                return File(ms.ToArray(), "application/pdf", "BookingConfirmation.pdf");
            }
        }

        // Method to send a confirmation email
        private void SendConfirmationEmail(BookingViewModel booking)
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
                    Customer customerDetails = getCurrentUser();


                    // Define the email subject and body
                    var subject = "Booking Confirmation - Travel Experts";
                        var body = $@"
                            Dear {customerDetails.CustFirstName},

                            Thank you for your booking! Here are your booking details:
                            - Booking Number: {booking.BookingNumber}
                            - Package: {booking.Package.Name}
                            - Booking Date: {booking.BookingDate:MM/dd/yyyy}
                            - Traveler Count: {booking.TravelersCount}

                            We look forward to serving you.

                            Best regards,
                            Travel Agency Team
                        ";

                        // Create and send the email
                        var mailMessage = new MailMessage(senderEmail,customerDetails.CustEmail, subject, body);
                        smtpClient.Send(mailMessage);

                        Console.WriteLine("Booking confirmation email sent successfully.");
                   
                }
            }
            catch (Exception ex)
            {
                // Log any errors that occur during the email-sending process
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }


        private string GenerateBookingNumber()
        {
            var random = new Random();
            var randomDigits = random.Next(10000, 99999); // Generate a random 5-digit number
            return $"GR4{randomDigits}";
        }

        public Customer getCurrentUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userName = User.Identity.Name;

                var customer = _context.Customers.FirstOrDefault(cus => cus.CustEmail == userName);
                ViewData["FirstName"] = customer.CustFirstName;
                ViewData["ProfilePicture"] = customer.ProfileImagePath;
                return customer;
            }
            return null;

        }


    }
}
