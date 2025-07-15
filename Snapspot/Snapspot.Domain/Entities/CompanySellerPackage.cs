using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Domain.Entities
{
    public class CompanySellerPackage
    {
        public Guid CompaniesId { get; set; }
        public virtual Company Company { get; set; }
        public Guid SellerPackagesId { get; set; }
        public virtual SellerPackage SellerPackage { get; set; }
        public int RemainingDay { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation property for Transactions

    }
}
