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

namespace TravelExpertsMVC.Views
{
    public class BookingController : Controller
    {
        private readonly TravelExpertsContext _context;

        public BookingController(TravelExpertsContext context)
        {
            _context = context;
        }

        // GET: Booking
        public async Task<IActionResult> Index()
        {
            var travelExpertssContext = _context.Bookings.Include(b => b.Customer).Include(b => b.Package).Include(b => b.TripType);
            ViewBag.Packages = PackageManager.GetPackages(_context);
            //should update customer Id to that of signed in customer
            List <BookingViewModel> bookings = BookingManager.GetBookings(_context, 127);
            return View(bookings);
        }

        // GET: Booking/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BookingViewModel booking = BookingManager.GetBookingById(_context, (int)id);

            return View(booking);
        }

        // GET: Booking/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustAddress");
            ViewData["PackageId"] = new SelectList(_context.Packages, "PackageId", "PkgName");
            ViewData["TripTypeId"] = new SelectList(_context.TripTypes, "TripTypeId", "TripTypeId");
            return View();
        }

        // POST: Booking/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,BookingDate,BookingNo,TravelerCount,CustomerId,TripTypeId,PackageId,PaymentStatus,Balance,TotalPaid")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustAddress", booking.CustomerId);
            ViewData["PackageId"] = new SelectList(_context.Packages, "PackageId", "PkgName", booking.PackageId);
            ViewData["TripTypeId"] = new SelectList(_context.TripTypes, "TripTypeId", "TripTypeId", booking.TripTypeId);
            return View(booking);
        }

        

       
    }
}
