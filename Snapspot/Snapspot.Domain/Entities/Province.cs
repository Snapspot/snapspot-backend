using Snapspot.Domain.Base;
using System;
using System.Collections.Generic;

namespace Snapspot.Domain.Entities
{
    public class Province : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual ICollection<District> Districts { get; set; }

        public Province()
        {
            Districts = new HashSet<District>();
        }
    }
} 