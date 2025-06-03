using System;
using System.Collections.Generic;
using Snapspot.Application.Models.Districts;

namespace Snapspot.Application.Models.Provinces
{
    public class ProvinceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<DistrictDto> Districts { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
} 