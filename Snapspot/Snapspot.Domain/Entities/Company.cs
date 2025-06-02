using Snapspot.Domain.Base;
using System;
using System.Collections.Generic;

namespace Snapspot.Domain.Entities
{
    public class Company : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AvatarUrl { get; set; }
        public string PdfUrl { get; set; }
        public float Rating { get; set; }
        public bool IsApproved { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Agency> Agencies { get; set; }
        public virtual ICollection<SellerPackage> SellerPackages { get; set; }

        public Company()
        {
            Agencies = new HashSet<Agency>();
            SellerPackages = new HashSet<SellerPackage>();
        }
    }
} 