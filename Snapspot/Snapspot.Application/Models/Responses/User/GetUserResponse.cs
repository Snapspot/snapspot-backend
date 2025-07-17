using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.Models.Responses.User
{
    public class GetUserResponse
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Dob { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public string Bio { get; set; }
        public string AvatarUrl { get; set; }
    }
}
