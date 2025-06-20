using Snapspot.Domain.Base;
using System;
using System.Collections.Generic;

namespace Snapspot.Domain.Entities
{
    public class Spot : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public bool IsDeleted { get; set; }
        public Guid DistrictId { get; set; }
        public virtual District District { get; set; }
        public virtual ICollection<Agency> Agencies { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }

        public Spot()
        {
            Agencies = new HashSet<Agency>();
        }
    }
} 