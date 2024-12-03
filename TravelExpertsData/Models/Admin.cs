using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TravelExpertsData.Models;

[Table("Admin")]
public partial class Admin
{
    [Key]
    [Column("AdminID")]
    public int AdminId { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(15)]
    public string? PhoneNumber { get; set; }

    [StringLength(50)]
    public string Role { get; set; } = null!;

    [InverseProperty("Admin")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
