using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TravelExpertsData.Models;

[Table("products_packages")]
[Index("PackageId", Name = "IX_products_packages_PackageId")]
[Index("ProductId", Name = "IX_products_packages_ProductId")]
public partial class ProductsPackage
{
    [Key]
    [Column("ProductPackageID")]
    public int ProductPackageId { get; set; }

    public int ProductId { get; set; }

    public int PackageId { get; set; }

    [ForeignKey("PackageId")]
    [InverseProperty("ProductsPackages")]
    public virtual Package Package { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("ProductsPackages")]
    public virtual Product Product { get; set; } = null!;
}
