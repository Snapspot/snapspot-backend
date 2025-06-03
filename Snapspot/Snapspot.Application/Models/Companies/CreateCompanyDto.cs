using System;

namespace Snapspot.Application.Models.Companies
{
    public class CreateCompanyDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AvatarUrl { get; set; }
        public string PdfUrl { get; set; }
        public Guid UserId { get; set; }
    }
} 