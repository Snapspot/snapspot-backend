using Snapspot.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Domain.Entities
{
    public class Style : BaseEntity<Guid>
    {
        public string Category { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        // Navigation properties
        public virtual ICollection<StyleSpot> StyleSpots { get; set; }

        public Style()
        {
            StyleSpots = new HashSet<StyleSpot>();
        }
    }
}
