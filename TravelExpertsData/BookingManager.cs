using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelExpertsData.Models;
using TravelExpertsData.ViewModels;

namespace TravelExpertsData
{
    public class BookingManager
    {
        //get all packages
        public static List<BookingViewModel> GetBookings(TravelExpertsContext db, int customerID)
        {
            return (List<BookingViewModel>)db.Bookings.
                Include(booking => booking.Package)      // Explicitly load related Package
                .Include(booking => booking.TripType)     // Explicitly load related TripType
        .Select(booking => new BookingViewModel
        {
            BookingId = booking.BookingId,
            CustomerId = (int)booking.CustomerId,
            BookingNumber = booking.BookingNo,
            Package = new PackageViewModel { Id= booking.Package.PackageId, Name = booking.Package.PkgName, Description = booking.Package.PkgDesc, StartDate = (DateTime) booking.Package.PkgStartDate, EndDate = (DateTime)booking.Package.PkgEndDate, BasePrice = booking.Package.PkgBasePrice, AgencyCommission = (decimal)booking.Package.PkgAgencyCommission, ImagePath = booking.Package.ImagePath ?? "/images/destination5.jpg" },
            TripTypeName = booking.TripType.Ttname,
            TravelersCount = (int)booking.TravelerCount,
            BookingDate = booking.BookingDate,
            TotalPaid = booking.TotalPaid,
            Balance = booking.Balance,
            PaymentStatus = booking.PaymentStatus
        }).Where(customer => customer.CustomerId == customerID).ToList();

        }
        //get booking by Id
        public static BookingViewModel GetBookingById(TravelExpertsContext db, int bookingId)
        {
            Booking booking = db.Bookings
        .Include(b => b.Package) // Load Package
        .Include(b => b.TripType) // Load TripType
        .FirstOrDefault(b => b.BookingId == bookingId);
            BookingViewModel bookingViewModel = new BookingViewModel();
            if (booking == null)
            {
                return null;
            }
            else
            {
                bookingViewModel = new BookingViewModel
                {
                    BookingId = booking.BookingId,
                    CustomerId = (int)booking.CustomerId,
                    BookingNumber = booking.BookingNo,
                    Package = booking.Package != null
            ? new PackageViewModel
            {
                Id = booking.Package.PackageId,
                Name = booking.Package.PkgName,
                Description = booking.Package.PkgDesc,
                StartDate = booking.Package.PkgStartDate ?? DateTime.MinValue,
                EndDate = booking.Package.PkgEndDate ?? DateTime.MinValue,
                BasePrice = booking.Package.PkgBasePrice,
                AgencyCommission = booking.Package.PkgAgencyCommission ?? 0,
                ImagePath = booking.Package.ImagePath ?? "/images/destination5.jpg"
            }
            : null,
                    TripTypeName = booking.TripType.Ttname,
                    TravelersCount = (int)booking.TravelerCount,
                    BookingDate = booking.BookingDate,
                    TotalPaid = booking.TotalPaid,
                    Balance = booking.Balance,
                    PaymentStatus = booking.PaymentStatus
                };
                return bookingViewModel;
            }
        }
    }
}
