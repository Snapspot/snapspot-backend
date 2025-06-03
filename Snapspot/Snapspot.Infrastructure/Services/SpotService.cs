using Snapspot.Application.Models.Agencies;
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
    public class SpotService : ISpotService
    {
        private readonly ISpotRepository _spotRepository;

        public SpotService(ISpotRepository spotRepository)
        {
            _spotRepository = spotRepository;
        }

        public async Task<SpotDto> GetByIdAsync(Guid id)
        {
            var spot = await _spotRepository.GetByIdAsync(id);
            return spot != null ? MapToDto(spot) : null;
        }

        public async Task<IEnumerable<SpotDto>> GetAllAsync()
        {
            var spots = await _spotRepository.GetAllAsync();
            return spots.Select(MapToDto);
        }

        public async Task<IEnumerable<SpotDto>> GetByDistrictIdAsync(Guid districtId)
        {
            var spots = await _spotRepository.GetByDistrictIdAsync(districtId);
            return spots.Select(MapToDto);
        }

        public async Task<SpotDto> CreateAsync(CreateSpotDto createSpotDto)
        {
            var spot = new Spot
            {
                Name = createSpotDto.Name,
                Description = createSpotDto.Description,
                DistrictId = createSpotDto.DistrictId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _spotRepository.AddAsync(spot);
            await _spotRepository.SaveChangesAsync();

            return MapToDto(spot);
        }

        public async Task<SpotDto> UpdateAsync(Guid id, UpdateSpotDto updateSpotDto)
        {
            var spot = await _spotRepository.GetByIdAsync(id);
            if (spot == null)
                throw new Exception("Spot not found");

            spot.Name = updateSpotDto.Name;
            spot.Description = updateSpotDto.Description;
            spot.DistrictId = updateSpotDto.DistrictId;
            spot.UpdatedAt = DateTime.UtcNow;

            await _spotRepository.UpdateAsync(spot);
            await _spotRepository.SaveChangesAsync();

            return MapToDto(spot);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var spot = await _spotRepository.GetByIdAsync(id);
            if (spot == null)
                return false;

            await _spotRepository.DeleteAsync(spot);
            await _spotRepository.SaveChangesAsync();

            return true;
        }

        private static SpotDto MapToDto(Spot spot)
        {
            return new SpotDto
            {
                Id = spot.Id,
                Name = spot.Name,
                Description = spot.Description,
                DistrictId = spot.DistrictId,
                DistrictName = spot.District?.Name,
                ProvinceName = spot.District?.Province?.Name,
                Agencies = spot.Agencies?.Select(a => new AgencyDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Address = a.Address,
                    Fullname = a.Fullname,
                    PhoneNumber = a.PhoneNumber,
                    AvatarUrl = a.AvatarUrl,
                    Rating = a.Rating,
                    CompanyId = a.CompanyId,
                    CompanyName = a.Company?.Name,
                    SpotId = a.SpotId,
                    SpotName = spot.Name,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                    IsDeleted = a.IsDeleted
                }).ToList(),
                CreatedAt = spot.CreatedAt,
                UpdatedAt = spot.UpdatedAt,
                IsDeleted = spot.IsDeleted
            };
        }
    }
} 