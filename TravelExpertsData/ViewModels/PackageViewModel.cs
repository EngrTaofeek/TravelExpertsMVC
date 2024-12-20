﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelExpertsData.ViewModels
{
    public class PackageViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal BasePrice { get; set; }
        public decimal AgencyCommission {  get; set; }
        public decimal TotalPrice { get; set; }
        public string ImagePath { get; set; }
    }
}
