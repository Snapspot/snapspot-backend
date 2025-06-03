using System;

namespace Snapspot.Application.Models.Spots
{
    public class CreateSpotDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid DistrictId { get; set; }
    }
} 