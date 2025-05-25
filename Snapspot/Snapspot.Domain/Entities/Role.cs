using Snapspot.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Domain.Entities
{
    public class Role : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
