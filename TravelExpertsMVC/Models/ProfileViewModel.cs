using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using TravelExpertsMVC.Controllers;

namespace TravelExpertsMVC.Models
{
    public class ProfileViewModel
    {
        [Required]
        [StringLength(25)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(25)]
        public string LastName { get; set; } = null!;

        [Required]
        [StringLength(25)]
        public string Address { get; set; } = null!;

        [Required]
        [StringLength(25)]
        public string City { get; set; } = null!;

        [Required]
        [StringLength(25)]
        public string Prov { get; set; } = null!;

        [Required]
        [StringLength(25)]
        public string PostalCode { get; set; } = null!;

        [Required]
        [StringLength(25)]
        public string Country { get; set; } = null!;

        [Required]
        [StringLength(25)]
        [DataType(DataType.PhoneNumber)]
        public string HomePhone { get; set; } = null!;

        [Required]
        [StringLength(25)]
        [DataType(DataType.PhoneNumber)]
        public string BusPhone { get; set; } = null!;

        [Required]
        [StringLength(25)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(25)]
        public string Postal { get; set; } = null!;

        [Required]
        [StringLength(25)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required]
        [StringLength(25)]
        [DataType(DataType.Password)]
        public string PasswordReset { get; set; } = null!;

        [Required]
        public IFormFile ProfilePicture { get; set; }

    }
}
