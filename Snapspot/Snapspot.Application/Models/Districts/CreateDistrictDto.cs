using System;

namespace Snapspot.Application.Models.Districts
{
    public class CreateDistrictDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ProvinceId { get; set; }
    }
} 