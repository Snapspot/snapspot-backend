using System;

namespace Snapspot.Application.Models.Districts
{
    public class UpdateDistrictDto
    {
        public string Name { get; set; }
        public Guid ProvinceId { get; set; }
    }
} 