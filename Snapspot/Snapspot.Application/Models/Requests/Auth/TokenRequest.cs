using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.Models.Requests.Auth
{
    public class TokenRequest
    {
        public Guid UserId { get; set; }
        public string RefreshToken { get; set; }
    }
}
