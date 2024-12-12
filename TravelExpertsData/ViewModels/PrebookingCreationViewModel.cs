using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelExpertsData.Models;

namespace TravelExpertsData.ViewModels
{
    public class PrebookingCreationViewModel
    {
        public PackageViewModel Package { get; set; }
        public List<CreditCardViewModel> CreditCards { get; set; }
        public List<TripType> TripTypes { get; set; }
    }
}
