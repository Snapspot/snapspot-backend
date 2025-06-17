using Microsoft.EntityFrameworkCore;
using Snapspot.Application.Models.Agencies;
using Snapspot.Application.Models.AgencyServices;
using Snapspot.Application.Repositories;
using Snapspot.Application.Services;
using Snapspot.Domain.Entities;
using Snapspot.Infrastructure.Persistence.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snapspot.Infrastructure.Services
{
    public class AgencyManagementService : IAgencyManagementService
    {
        private readonly IAgencyRepository _agencyRepository;

        public AgencyManagementService(IAgencyRepository agencyRepository)
        {
            _agencyRepository = agencyRepository;
        }

        public async Task<AgencyDto> GetByIdAsync(Guid id)
        {
            var agency = await _agencyRepository.GetByIdAsync(id);
            return agency != null ? MapToDto(agency) : null;
        }

        public async Task<IEnumerable<AgencyDto>> GetAllAsync()
        {
            var agencies = await _agencyRepository.GetAllAsync();
            return agencies.Select(MapToDto);
        }

        public async Task<IEnumerable<AgencyDto>> GetByCompanyIdAsync(Guid companyId)
        {
            var agencies = await _agencyRepository.GetByCompanyIdAsync(companyId);
            return agencies.Select(MapToDto);
        }

        public async Task<IEnumerable<AgencyDto>> GetBySpotIdAsync(Guid spotId)
        {
            var agencies = await _agencyRepository.GetBySpotIdAsync(spotId);
            return agencies.Select(MapToDto);
        }


        public Task<AgencyDto> CreateAsync(CreateAgencyDto createAgencyDto)
        {
            throw new NotImplementedException();
        }
        //public async Task<AgencyDto> CreateAsync(CreateAgencyDto createAgencyDto)
        //{
        //    // Validate Company exists
        //    var company = await _context.Companies
        //        .FirstOrDefaultAsync(c => c.Id == createAgencyDto.CompanyId && !c.IsDeleted);
        //    if (company == null)
        //        throw new Exception($"Company with ID '{createAgencyDto.CompanyId}' not found");

        //    // Validate Spot exists
        //    var spot = await _context.Spots
        //        .FirstOrDefaultAsync(s => s.Id == createAgencyDto.SpotId && !s.IsDeleted);
        //    if (spot == null)
        //        throw new Exception($"Spot with ID '{createAgencyDto.SpotId}' not found");

        //    var agency = new Agency
        //    {
        //        Name = createAgencyDto.Name,
        //        Address = createAgencyDto.Address,
        //        Fullname = createAgencyDto.Fullname,
        //        PhoneNumber = createAgencyDto.PhoneNumber,
        //        AvatarUrl = createAgencyDto.AvatarUrl,
        //        Rating = 0,
        //        CompanyId = createAgencyDto.CompanyId,
        //        SpotId = createAgencyDto.SpotId,
        //        CreatedAt = DateTime.UtcNow,
        //        UpdatedAt = DateTime.UtcNow,
        //        IsDeleted = false
        //    };

        //    await _agencyRepository.AddAsync(agency);
        //    await _agencyRepository.SaveChangesAsync();

        //    return MapToDto(agency);
        //}

        public async Task<AgencyDto> UpdateAsync(Guid id, UpdateAgencyDto updateAgencyDto)
        {
            var agency = await _agencyRepository.GetByIdAsync(id);
            if (agency == null)
                throw new Exception("Agency not found");

            // Validate Spot exists
            //if (agency.SpotId != updateAgencyDto.SpotId)
            //{
            //    var spot = await _context.Spots
            //        .FirstOrDefaultAsync(s => s.Id == updateAgencyDto.SpotId && !s.IsDeleted);
            //    if (spot == null)
            //        throw new Exception($"Spot with ID '{updateAgencyDto.SpotId}' not found");
            //}

            agency.Name = updateAgencyDto.Name;
            agency.Address = updateAgencyDto.Address;
            agency.Fullname = updateAgencyDto.Fullname;
            agency.PhoneNumber = updateAgencyDto.PhoneNumber;
            agency.AvatarUrl = updateAgencyDto.AvatarUrl;
            agency.SpotId = updateAgencyDto.SpotId;
            agency.UpdatedAt = DateTime.UtcNow;

            await _agencyRepository.UpdateAsync(agency);
            await _agencyRepository.SaveChangesAsync();

            return MapToDto(agency);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var agency = await _agencyRepository.GetByIdAsync(id);
            if (agency == null)
                return false;

            await _agencyRepository.DeleteAsync(agency);
            await _agencyRepository.SaveChangesAsync();

            return true;
        }

        private static AgencyDto MapToDto(Agency agency)
        {
            return new AgencyDto
            {
                Id = agency.Id,
                Name = agency.Name,
                Address = agency.Address,
                Fullname = agency.Fullname,
                PhoneNumber = agency.PhoneNumber,
                AvatarUrl = agency.AvatarUrl,
                Rating = agency.Rating,
                CompanyId = agency.CompanyId,
                CompanyName = agency.Company?.Name,
                SpotId = agency.SpotId,
                SpotName = agency.Spot?.Name,
                Services = agency.Services?.Select(s => new AgencyServiceDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Color = s.Color
                }).ToList(),
                CreatedAt = agency.CreatedAt,
                UpdatedAt = agency.UpdatedAt,
                IsDeleted = agency.IsDeleted
            };
        }

    }
} 