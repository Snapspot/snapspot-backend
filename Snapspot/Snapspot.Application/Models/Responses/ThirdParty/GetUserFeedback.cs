using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.Models.Responses.ThirdParty
{
    public class GetUserFeedback
    {
        public string FullName { get; set; } = string.Empty;
        public string AgencyName { get; set; } = string.Empty;
        public int Rating { get; set; } = 0;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
}
