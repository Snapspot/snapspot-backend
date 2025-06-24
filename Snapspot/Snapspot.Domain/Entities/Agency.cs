using Snapspot.Domain.Base;
using System;
using System.Collections.Generic;

namespace Snapspot.Domain.Entities
{
    public class Agency : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Fullname { get; set; }
        public string PhoneNumber { get; set; }
        public string AvatarUrl { get; set; }
        public float Rating { get; set; }
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public Guid SpotId { get; set; }
        public virtual Spot Spot { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<AgencyService> Services { get; set; }
        public string Description { get; set; }

        public Agency()
        {
            Feedbacks = new HashSet<Feedback>();
            Services = new HashSet<AgencyService>();
        }
    }
} 