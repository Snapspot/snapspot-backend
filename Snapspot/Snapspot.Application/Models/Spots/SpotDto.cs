using System;
using System.Collections.Generic;
using Snapspot.Application.Models.Agencies;
using Snapspot.Application.Models.Styles;

namespace Snapspot.Application.Models.Spots
{
    public class SpotDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public Guid DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string ProvinceName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }
        public List<StyleDto> Styles { get; set; } = new List<StyleDto>();
        public string? Time { get; set; }
    }
} 