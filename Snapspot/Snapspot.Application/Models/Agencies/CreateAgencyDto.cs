using System;
using System.ComponentModel.DataAnnotations;

namespace Snapspot.Application.Models.Agencies
{
    public class CreateAgencyDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        [StringLength(100)]
        public string Fullname { get; set; }

        [Required]
        [StringLength(20)]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [Url]
        public string AvatarUrl { get; set; }

        [Required]
        public Guid SpotId { get; set; }

        public string Description { get; set; }

        [Required]
        public Guid[] AgencyServiceIds { get; set; }
    }
} 