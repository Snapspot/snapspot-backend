using Snapspot.Application.Models.AgencyServices;
using Snapspot.Application.Repositories;
using Snapspot.Application.Services;
using Snapspot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snapspot.Infrastructure.Services
{
    public class AgencyServiceService : IAgencyServiceService
    {
        private readonly IAgencyServiceRepository _agencyServiceRepository;
        private readonly IAgencyRepository _agencyRepository;

        public AgencyServiceService(IAgencyServiceRepository agencyServiceRepository, IAgencyRepository agencyRepository)
        {
            _agencyServiceRepository = agencyServiceRepository;
            _agencyRepository = agencyRepository;
        }

        public async Task<AgencyServiceDto> GetByIdAsync(Guid id)
        {
            var service = await _agencyServiceRepository.GetByIdAsync(id);
            return service != null ? MapToDto(service) : null;
        }

        public async Task<IEnumerable<AgencyServiceDto>> GetAllAsync()
        {
            var services = await _agencyServiceRepository.GetAllAsync();
            return services.Select(MapToDto);
        }

        public async Task<IEnumerable<AgencyServiceDto>> GetByAgencyIdAsync(Guid agencyId)
        {
            var services = await _agencyServiceRepository.GetByAgencyIdAsync(agencyId);
            return services.Select(MapToDto);
        }

        public async Task<AgencyServiceDto> CreateAsync(CreateAgencyServiceDto createAgencyServiceDto)
        {
            var service = new AgencyService
            {
                Name = createAgencyServiceDto.Name,
                Color = createAgencyServiceDto.Color,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _agencyServiceRepository.AddAsync(service);
            await _agencyServiceRepository.SaveChangesAsync();

            return MapToDto(service);
        }

        public async Task<AgencyServiceDto> UpdateAsync(Guid id, UpdateAgencyServiceDto updateAgencyServiceDto)
        {
            var service = await _agencyServiceRepository.GetByIdAsync(id);
            if (service == null)
                throw new Exception("Service not found");

            service.Name = updateAgencyServiceDto.Name;
            service.Color = updateAgencyServiceDto.Color;
            service.UpdatedAt = DateTime.UtcNow;

            await _agencyServiceRepository.UpdateAsync(service);
            await _agencyServiceRepository.SaveChangesAsync();

            return MapToDto(service);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var service = await _agencyServiceRepository.GetByIdAsync(id);
            if (service == null)
                return false;

            await _agencyServiceRepository.DeleteAsync(service);
            await _agencyServiceRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddToAgencyAsync(Guid serviceId, Guid agencyId)
        {
            if (!await _agencyServiceRepository.ExistsAsync(serviceId))
                throw new Exception("Service not found");

            if (!await _agencyRepository.ExistsAsync(agencyId))
                throw new Exception("Agency not found");

            await _agencyServiceRepository.AddToAgencyAsync(serviceId, agencyId);
            await _agencyServiceRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveFromAgencyAsync(Guid serviceId, Guid agencyId)
        {
            if (!await _agencyServiceRepository.ExistsAsync(serviceId))
                throw new Exception("Service not found");

            if (!await _agencyRepository.ExistsAsync(agencyId))
                throw new Exception("Agency not found");

            await _agencyServiceRepository.RemoveFromAgencyAsync(serviceId, agencyId);
            await _agencyServiceRepository.SaveChangesAsync();

            return true;
        }

        private static AgencyServiceDto MapToDto(AgencyService service)
        {
            return new AgencyServiceDto
            {
                Id = service.Id,
                Name = service.Name,
                Color = service.Color,
                IsDeleted = service.IsDeleted,
                CreatedAt = service.CreatedAt,
                UpdatedAt = service.UpdatedAt
            };
        }
    }
} 