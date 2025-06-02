using Snapspot.Domain.Base;
using System;
using System.Collections.Generic;

namespace Snapspot.Domain.Entities
{
    public class AgencyService : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual ICollection<Agency> Agencies { get; set; }

        public AgencyService()
        {
            Agencies = new HashSet<Agency>();
        }
    }
} 