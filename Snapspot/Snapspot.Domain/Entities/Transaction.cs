using Snapspot.Domain.Base;
using System;

namespace Snapspot.Domain.Entities
{
    public class Transaction : BaseEntity<Guid>
    {
        public Guid SellerPackageId { get; set; }
        public virtual SellerPackage SellerPackage { get; set; }

        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public string TransactionCode { get; set; }
        public decimal Amount { get; set; }
        public string PaymentType { get; set; }
        public DateTime CreatedAt { get; set; }

        public Transaction()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
} 