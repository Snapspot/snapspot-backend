using Snapspot.Domain.Base;
using System;
using System.Collections.Generic;

namespace Snapspot.Domain.Entities
{
    public class District : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ProvinceId { get; set; }
        public virtual Province Province { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<Spot> Spots { get; set; }

        public District()
        {
            Spots = new HashSet<Spot>();
        }
    }
} 