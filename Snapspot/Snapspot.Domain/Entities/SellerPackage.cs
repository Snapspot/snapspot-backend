using Snapspot.Domain.Base;
using System;
using System.Collections.Generic;

namespace Snapspot.Domain.Entities
{
    public class SellerPackage : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxAgency { get; set; }
        public decimal Price { get; set; }
        public long SellingCount { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<CompanySellerPackage> CompanySellerPackages { get; set; }

        public SellerPackage()
        {
            Transactions = new HashSet<Transaction>();
            CompanySellerPackages = new HashSet<CompanySellerPackage>();
        }
    }
} 