﻿using System.ComponentModel.DataAnnotations;

namespace TravelExpertsMVC.Models
{
    public class RegisterViewModel
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
        [RegularExpression(@"^[ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ] ?\d[ABCEGHJKLMNPRSTVWXYZ]\d$", ErrorMessage = "Invalid postal code")]
        public string CustPostal { get; set; } = null!;

        [Required]
        [StringLength(25)]
        public string? CustCountry { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid phone number")]
        public string? CustHomePhone { get; set; }

        [StringLength(20)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid phone number")]
        public string CustBusPhone { get; set; } = null!;

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string CustEmail { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password does not match")]
        public string ConfirmPassword { get; set; }

        public IFormFile? ProfileImage { get; set; }
        public string? ProfileImagePath { get; set; }
    }
}
