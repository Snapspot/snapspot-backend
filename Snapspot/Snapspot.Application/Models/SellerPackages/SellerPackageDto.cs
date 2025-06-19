using System;
using System.Collections.Generic;
using Snapspot.Application.Models.Companies;

namespace Snapspot.Application.Models.SellerPackages
{
    public class SellerPackageDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxAgency { get; set; }
        public decimal Price { get; set; }
        public long SellingCount { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<CompanyDto> Companies { get; set; }
    }
} 