using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelExpertsData.Models
{
    public class User:IdentityUser
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string FullName { get; set; }
        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
