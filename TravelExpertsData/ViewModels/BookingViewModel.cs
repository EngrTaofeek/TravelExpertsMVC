using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelExpertsData.ViewModels
{
    public class BookingViewModel
    {
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public DateTime? BookingDate { get; set; }
        public string BookingNumber { get; set; }
        public string TripTypeName { get; set; }
        public int TravelersCount { get; set; }
        public string PaymentStatus { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalPaid { get; set; }
        public PackageViewModel Package { get; set; }

    }
}
