using System;
using System.ComponentModel.DataAnnotations;

namespace Snapspot.Application.Models.Spots
{
    public class CreateSpotDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
        public double? Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
        public double? Longitude { get; set; }

        [Required]
        public Guid DistrictId { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(300)]
        public string ImageUrl { get; set; }
        public string? Time { get; set; }
    }
} 