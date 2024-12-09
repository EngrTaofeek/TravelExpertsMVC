using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelExpertsData.Models;
using TravelExpertsData.ViewModels;

namespace TravelExpertsData
{
    public class PackageManager
    {
        //get all packages
        public static List<PackageViewModel> GetPackages(TravelExpertssContext db)
        {
            return db.Packages.Select(package => new PackageViewModel
            {
                Id = package.PackageId,
                Name = package.PkgName,
                Description = package.PkgDesc,
                AgencyCommission = package.PkgAgencyCommission ?? 0,
                BasePrice = package.PkgBasePrice,
                StartDate = (DateTime)package.PkgStartDate,
                EndDate = (DateTime)package.PkgEndDate,
                ImagePath = package.ImagePath

            }).ToList();

        }
         //get package by Id
        public static PackageViewModel GetPackageById(TravelExpertssContext db,int packageId) {
            Package package = db.Packages.Find(packageId);
            if (package == null) {
                return null;
            }
            else
            {
                return new PackageViewModel
                {
                    Id = package.PackageId,
                    Name = package.PkgName,
                    Description = package.PkgDesc,
                    AgencyCommission = package.PkgAgencyCommission ?? 0,
                    BasePrice = package.PkgBasePrice,
                    StartDate = (DateTime)package.PkgStartDate,
                    EndDate = (DateTime)package.PkgEndDate,
                    ImagePath = package.ImagePath

                };
            }
        }

    }
}
