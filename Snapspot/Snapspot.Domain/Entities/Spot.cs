using Snapspot.Domain.Base;
using System;
using System.Collections.Generic;

namespace Snapspot.Domain.Entities
{
    public class Spot : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public Guid DistrictId { get; set; }
        public virtual District District { get; set; }
        public virtual ICollection<Agency> Agencies { get; set; }

        public Spot()
        {
            Agencies = new HashSet<Agency>();
        }
    }
} 