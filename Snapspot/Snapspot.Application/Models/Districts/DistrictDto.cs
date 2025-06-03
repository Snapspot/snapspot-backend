using System;
using System.Collections.Generic;
using Snapspot.Application.Models.Spots;

namespace Snapspot.Application.Models.Districts
{
    public class DistrictDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public ICollection<SpotDto> Spots { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
} 