using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelExpertsData;
using TravelExpertsData.Models;
using TravelExpertsData.ViewModels;

namespace TravelExpertsMVC.Controllers
{
    public class BookingController : Controller
    {
        private readonly TravelExpertsContext _context;
        private int customerId = 127;

        public BookingController(TravelExpertsContext context)
        {
            _context = context;
        }

        // GET: Booking
        public async Task<IActionResult> Index()
        {
            // Set the current page's controller and action in ViewData
            ViewData["ActiveController"] = "Booking";
            ViewData["ActiveAction"] = "Index";
            var travelExpertssContext = _context.Bookings.Include(b => b.Customer).Include(b => b.Package).Include(b => b.TripType);
            ViewBag.Packages = PackageManager.GetPackages(_context);
            //should update customer Id to that of signed in customer
            List<BookingViewModel> bookings = BookingManager.GetBookings(_context, customerId);
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

            BookingViewModel booking = BookingManager.GetBookingById(_context, (int)id);

            return View(booking);
        }

        // GET: Booking/Create
        public IActionResult Create(int? packageId)
        {
            // Set the current page's controller and action in ViewData
            ViewData["ActiveController"] = "Booking";
            ViewData["ActiveAction"] = "Create";
            PackageViewModel package = PackageManager.GetPackageById(_context,(int) packageId);
            //hardcoded customerId
            List<CreditCardViewModel> creditCards = CreditCardManager.GetCreditCardsForCustomer(_context, customerId);
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
            // Retrieve the selected credit card
            //replace hardcoded selected credit card
            //var selectedCreditCardId = booking.SelectedCreditCardId;
            var selectedCreditCardId = booking.SelectedCreditCardId;
            var packageTotalPrice = booking.Package.TotalPrice;

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
                CustomerId = customerId,
                BookingDate = DateTime.Now,
                BookingNo = bookingNumber,
                //change tiptype from hard coded value
                TripTypeId = booking.TripTypeId,
                TravelerCount = booking.TravelersCount,
                PaymentStatus = "Paid",
                Balance = 0,
                TotalPaid = packageTotalPrice,
                PackageId = booking.Package.Id
            };

            _context.Bookings.Add(newBooking);
            _context.SaveChanges();

            //return RedirectToAction($"ThankYou?bookingNumber={bookingNumber}");
            return RedirectToAction("ThankYou", new { bookingNumber });

        }

        private string GenerateBookingNumber()
        {
            var random = new Random();
            var randomDigits = random.Next(10000, 99999); // Generate a random 5-digit number
            return $"GR4{randomDigits}";
        }


        public IActionResult ThankYou(string bookingNumber)
        {
            var booking = _context.Bookings.Include(b => b.Package).FirstOrDefault(b => b.BookingNo == bookingNumber);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

    }
}
