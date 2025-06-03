using System;
using System.Collections.Generic;
using Snapspot.Application.Models.Agencies;

namespace Snapspot.Application.Models.Companies
{
    public class CompanyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AvatarUrl { get; set; }
        public string PdfUrl { get; set; }
        public float Rating { get; set; }
        public bool IsApproved { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public ICollection<AgencyDto> Agencies { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
} 