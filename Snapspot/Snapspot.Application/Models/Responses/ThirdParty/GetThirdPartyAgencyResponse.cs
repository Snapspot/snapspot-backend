using Snapspot.Application.Models.Agencies;
using Snapspot.Application.Models.AgencyServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.Models.Responses.ThirdParty
{
    public class GetThirdPartyAgencyResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Fullname { get; set; }
        public string PhoneNumber { get; set; }
        public string AvatarUrl { get; set; }
        public float Rating { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public Guid SpotId { get; set; }
        public string SpotName { get; set; }
        public ICollection<AgencyServiceDto> Services { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<FeedbackDto> Feedbacks { get; set; }
        public string Description { get; set; }
    }
}
