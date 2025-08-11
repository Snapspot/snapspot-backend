using Snapspot.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Domain.Entities
{
    public class StyleSpot : BaseEntity<Guid>
    {
        public Guid StyleId { get; set; }
        public Guid SpotId { get; set; }

        // Navigation properties
        public virtual Style Style { get; set; }
        public virtual Spot Spot { get; set; }
    }
}
