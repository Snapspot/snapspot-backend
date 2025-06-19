using System;

namespace Snapspot.Application.Models.SellerPackages
{
    public class CreateSellerPackageDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxAgency { get; set; }
        public decimal Price { get; set; }
    }
} 