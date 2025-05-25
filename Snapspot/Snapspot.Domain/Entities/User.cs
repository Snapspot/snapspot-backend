using Snapspot.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Domain.Entities
{
    public class User : BaseEntity<Guid>
    {
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string Password { get; set; }
        public DateTime Dob { get; set; }
        public string PhoneNumber { get; set; }
        public string AvartaUrl { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}
