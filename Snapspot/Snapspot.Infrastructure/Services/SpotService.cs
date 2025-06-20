using Snapspot.Application.Models.Agencies;
using Snapspot.Application.Models.Spots;
using Snapspot.Application.Repositories;
using Snapspot.Application.Services;
using Snapspot.Domain.Entities;
using Snapspot.Infrastructure.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snapspot.Infrastructure.Services
{
    public class SpotService : ISpotService
    {
        private readonly ISpotRepository _spotRepository;
        private readonly AppDbContext _context;

        public SpotService(ISpotRepository spotRepository, AppDbContext context)
        {
            _spotRepository = spotRepository;
            _context = context;
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
            try
            {
                // Validate District exists
                var districtExists = await _context.Districts.AnyAsync(d => d.Id == createSpotDto.DistrictId && !d.IsDeleted);
                if (!districtExists)
                {
                    throw new Exception($"District with ID '{createSpotDto.DistrictId}' not found");
                }

                var spot = new Spot
                {
                    Name = createSpotDto.Name,
                    Description = createSpotDto.Description,
                    Latitude = createSpotDto.Latitude,
                    Longitude = createSpotDto.Longitude,
                    DistrictId = createSpotDto.DistrictId,
                    Address = createSpotDto.Address,
                    ImageUrl = createSpotDto.ImageUrl,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                await _spotRepository.AddAsync(spot);
                await _spotRepository.SaveChangesAsync();

                return MapToDto(spot);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating spot: {ex.Message}", ex);
            }
        }

        public async Task<SpotDto> UpdateAsync(Guid id, UpdateSpotDto updateSpotDto)
        {
            try
            {
                var spot = await _spotRepository.GetByIdAsync(id);
                if (spot == null)
                    throw new Exception("Spot not found");

                // Validate District exists if it's being changed
                if (spot.DistrictId != updateSpotDto.DistrictId)
                {
                    var districtExists = await _context.Districts.AnyAsync(d => d.Id == updateSpotDto.DistrictId && !d.IsDeleted);
                    if (!districtExists)
                    {
                        throw new Exception($"District with ID '{updateSpotDto.DistrictId}' not found");
                    }
                }

                spot.Name = updateSpotDto.Name;
                spot.Description = updateSpotDto.Description;
                spot.Latitude = updateSpotDto.Latitude;
                spot.Longitude = updateSpotDto.Longitude;
                spot.DistrictId = updateSpotDto.DistrictId;
                spot.Address = updateSpotDto.Address;
                spot.ImageUrl = updateSpotDto.ImageUrl;
                spot.UpdatedAt = DateTime.UtcNow;

                await _spotRepository.UpdateAsync(spot);
                await _spotRepository.SaveChangesAsync();

                return MapToDto(spot);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating spot: {ex.Message}", ex);
            }
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
                Latitude = spot.Latitude,
                Longitude = spot.Longitude,
                DistrictId = spot.DistrictId,
                DistrictName = spot.District?.Name,
                ProvinceName = spot.District?.Province?.Name,
                Address = spot.Address,
                ImageUrl = spot.ImageUrl,
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