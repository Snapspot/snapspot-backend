using System;
using System.Collections.Generic;
using Snapspot.Application.Models.AgencyServices;

namespace Snapspot.Application.Models.Agencies
{
    public class AgencyDto
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
    }
} 