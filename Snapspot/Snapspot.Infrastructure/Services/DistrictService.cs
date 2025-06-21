using Snapspot.Application.Models.Districts;
using Snapspot.Application.Models.Responses.Auth;
using Snapspot.Application.Models.Spots;
using Snapspot.Application.Repositories;
using Snapspot.Application.Services;
using Snapspot.Domain.Entities;
using Snapspot.Shared.Common;
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

        public async Task<ApiResponse<IEnumerable<DistrictDto>>> GetAllAsync()
        {
            var districts = await _districtRepository.GetAllAsync();
            var apiResponse = new ApiResponse<IEnumerable<DistrictDto>>
            {
                Data = districts.Select(MapToDto),
                Success = true,
                Message = "Districts retrieved successfully"
            };
            return apiResponse;
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
            district.Description = updateDistrictDto.Description;
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
                Description = district.Description,
                ProvinceId = district.ProvinceId,
                ProvinceName = district.Province?.Name,
                CreatedAt = district.CreatedAt,
                UpdatedAt = district.UpdatedAt,
                IsDeleted = district.IsDeleted
            };
        }
    }
} 