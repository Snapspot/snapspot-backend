using Snapspot.Application.Models.Districts;
using Snapspot.Application.Models.Spots;
using Snapspot.Application.Repositories;
using Snapspot.Application.Services;
using Snapspot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snapspot.Infrastructure.Services
{
    public class DistrictService : IDistrictService
    {
        private readonly IDistrictRepository _districtRepository;
        private readonly IProvinceRepository _provinceRepository;

        public DistrictService(IDistrictRepository districtRepository, IProvinceRepository provinceRepository)
        {
            _districtRepository = districtRepository;
            _provinceRepository = provinceRepository;
        }

        public async Task<DistrictDto> GetByIdAsync(Guid id)
        {
            var district = await _districtRepository.GetByIdAsync(id);
            return district != null ? MapToDto(district) : null;
        }

        public async Task<IEnumerable<DistrictDto>> GetAllAsync()
        {
            var districts = await _districtRepository.GetAllAsync();
            return districts.Select(MapToDto);
        }

        public async Task<IEnumerable<DistrictDto>> GetByProvinceIdAsync(Guid provinceId)
        {
            var districts = await _districtRepository.GetByProvinceIdAsync(provinceId);
            return districts.Select(MapToDto);
        }

        public async Task<DistrictDto> CreateAsync(CreateDistrictDto createDistrictDto)
        {
            if (!await _provinceRepository.ExistsAsync(createDistrictDto.ProvinceId))
                throw new Exception("Province not found");

            var district = new District
            {
                Name = createDistrictDto.Name,
                Description = createDistrictDto.Description,
                ProvinceId = createDistrictDto.ProvinceId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _districtRepository.AddAsync(district);
            await _districtRepository.SaveChangesAsync();

            return MapToDto(district);
        }

        public async Task<DistrictDto> UpdateAsync(Guid id, UpdateDistrictDto updateDistrictDto)
        {
            var district = await _districtRepository.GetByIdAsync(id);
            if (district == null)
                throw new Exception("District not found");

            if (!await _provinceRepository.ExistsAsync(updateDistrictDto.ProvinceId))
                throw new Exception("Province not found");

            district.Name = updateDistrictDto.Name;
            district.ProvinceId = updateDistrictDto.ProvinceId;
            district.UpdatedAt = DateTime.UtcNow;

            await _districtRepository.UpdateAsync(district);
            await _districtRepository.SaveChangesAsync();

            return MapToDto(district);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var district = await _districtRepository.GetByIdAsync(id);
            if (district == null)
                return false;

            await _districtRepository.DeleteAsync(district);
            await _districtRepository.SaveChangesAsync();

            return true;
        }

        private static DistrictDto MapToDto(District district)
        {
            return new DistrictDto
            {
                Id = district.Id,
                Name = district.Name,
                ProvinceId = district.ProvinceId,
                ProvinceName = district.Province?.Name,
                Spots = district.Spots?.Select(s => new SpotDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    DistrictId = s.DistrictId,
                    DistrictName = district.Name,
                    ProvinceName = district.Province?.Name,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    IsDeleted = s.IsDeleted
                }).ToList(),
                CreatedAt = district.CreatedAt,
                UpdatedAt = district.UpdatedAt,
                IsDeleted = district.IsDeleted
            };
        }
    }
} 