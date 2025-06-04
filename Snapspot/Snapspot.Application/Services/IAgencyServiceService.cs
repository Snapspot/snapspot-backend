using Snapspot.Application.Models.AgencyServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Application.Services
{
    public interface IAgencyServiceService
    {
        Task<AgencyServiceDto> GetByIdAsync(Guid id);
        Task<IEnumerable<AgencyServiceDto>> GetAllAsync();
        Task<IEnumerable<AgencyServiceDto>> GetByAgencyIdAsync(Guid agencyId);
        Task<AgencyServiceDto> CreateAsync(CreateAgencyServiceDto createAgencyServiceDto);
        Task<AgencyServiceDto> UpdateAsync(Guid id, UpdateAgencyServiceDto updateAgencyServiceDto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> AddToAgencyAsync(Guid serviceId, Guid agencyId);
        Task<bool> RemoveFromAgencyAsync(Guid serviceId, Guid agencyId);
    }
} 