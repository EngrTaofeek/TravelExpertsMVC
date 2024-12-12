using System.ComponentModel.DataAnnotations;

namespace TravelExpertsMVC.Models
{
    public class ProfileViewModel
    {

        [Required]
        [StringLength(25)]
        public string CustFirstName { get; set; } = null!;

        [Required]
        [StringLength(25)]
        public string CustLastName { get; set; } = null!;

        [Required]
        [StringLength(75)]
        public string CustAddress { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string CustCity { get; set; } = null!;

        [Required]
        [StringLength(2)]
        public string CustProv { get; set; } = null!;

        [Required]
        [StringLength(7)]
        public string CustPostal { get; set; } = null!;

        [Required]
        [StringLength(25)]
        public string? CustCountry { get; set; }

        [Required]
        [StringLength(20)]
        public string? CustHomePhone { get; set; }

        [StringLength(20)]
        public string CustBusPhone { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string CustEmail { get; set; } = null!;

        public IFormFile? ProfileImage { get; set; }
        public string? ProfileImagePath { get; set; }
    }
}
