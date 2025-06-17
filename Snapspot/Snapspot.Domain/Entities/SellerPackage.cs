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
        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }

        public SellerPackage()
        {
            Companies = new HashSet<Company>();
            Transactions = new HashSet<Transaction>();
        }
    }
} 