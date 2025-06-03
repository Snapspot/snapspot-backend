using Snapspot.Application.Models.Districts;
using Snapspot.Application.Models.Provinces;
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
    public class ProvinceService : IProvinceService
    {
        private readonly IProvinceRepository _provinceRepository;

        public ProvinceService(IProvinceRepository provinceRepository)
        {
            _provinceRepository = provinceRepository;
        }

        public async Task<ProvinceDto> GetByIdAsync(Guid id)
        {
            var province = await _provinceRepository.GetByIdAsync(id);
            return province != null ? MapToDto(province) : null;
        }

        public async Task<IEnumerable<ProvinceDto>> GetAllAsync()
        {
            var provinces = await _provinceRepository.GetAllAsync();
            return provinces.Select(MapToDto);
        }

        public async Task<ProvinceDto> CreateAsync(CreateProvinceDto createProvinceDto)
        {
            var province = new Province
            {
                Name = createProvinceDto.Name,
                Description = createProvinceDto.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _provinceRepository.AddAsync(province);
            await _provinceRepository.SaveChangesAsync();

            return MapToDto(province);
        }

        public async Task<ProvinceDto> UpdateAsync(Guid id, UpdateProvinceDto updateProvinceDto)
        {
            var province = await _provinceRepository.GetByIdAsync(id);
            if (province == null)
                throw new Exception("Province not found");

            province.Name = updateProvinceDto.Name;
            province.Description = updateProvinceDto.Description;
            province.UpdatedAt = DateTime.UtcNow;

            await _provinceRepository.UpdateAsync(province);
            await _provinceRepository.SaveChangesAsync();

            return MapToDto(province);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var province = await _provinceRepository.GetByIdAsync(id);
            if (province == null)
                return false;

            await _provinceRepository.DeleteAsync(province);
            await _provinceRepository.SaveChangesAsync();

            return true;
        }

        private static ProvinceDto MapToDto(Province province)
        {
            return new ProvinceDto
            {
                Id = province.Id,
                Name = province.Name,
                Districts = province.Districts?.Select(d => new DistrictDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    ProvinceId = d.ProvinceId,
                    ProvinceName = province.Name,
                    Spots = d.Spots?.Select(s => new SpotDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Description = s.Description,
                        DistrictId = s.DistrictId,
                        DistrictName = d.Name,
                        ProvinceName = province.Name,
                        CreatedAt = s.CreatedAt,
                        UpdatedAt = s.UpdatedAt,
                        IsDeleted = s.IsDeleted
                    }).ToList(),
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt,
                    IsDeleted = d.IsDeleted
                }).ToList(),
                CreatedAt = province.CreatedAt,
                UpdatedAt = province.UpdatedAt,
                IsDeleted = province.IsDeleted
            };
        }
    }
} 