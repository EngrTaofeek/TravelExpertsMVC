using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelExpertsData.ViewModels
{
    public class ContactViewModel
    {
        public int AgencyId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public List<AgentViewModel> Agents { get; set; } = new List<AgentViewModel>();
    }
}
