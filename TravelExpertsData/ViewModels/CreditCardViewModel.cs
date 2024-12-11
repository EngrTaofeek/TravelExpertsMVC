using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelExpertsData.ViewModels
{
    public class CreditCardViewModel
    {
        public int CreditCardId { get; set; }
        public string CCName { get; set; }
        public string CCNumber { get; set; }
        public DateTime CCExpiry { get; set; }
        public int CustomerId { get; set; }
        public decimal Balance { get; set; }
        public string CreditCardImagePath { get; set; }
    }

}
