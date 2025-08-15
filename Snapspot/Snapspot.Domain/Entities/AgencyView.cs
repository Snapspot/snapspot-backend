using Snapspot.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Domain.Entities
{
    public class AgencyView : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public Guid AgencyId { get; set; }

        public DateTime ViewDate { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("AgencyId")]
        public virtual Agency Agency { get; set; }
    }
}
