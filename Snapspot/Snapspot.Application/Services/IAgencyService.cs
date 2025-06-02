using Snapspot.Application.Models.Agencies;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Application.Services
{
    public interface IAgencyService
    {
        Task<AgencyDto> GetByIdAsync(Guid id);
        Task<IEnumerable<AgencyDto>> GetAllAsync();
        Task<IEnumerable<AgencyDto>> GetByCompanyIdAsync(Guid companyId);
        Task<IEnumerable<AgencyDto>> GetBySpotIdAsync(Guid spotId);
        Task<AgencyDto> CreateAsync(CreateAgencyDto createAgencyDto);
        Task<AgencyDto> UpdateAsync(Guid id, UpdateAgencyDto updateAgencyDto);
        Task<bool> DeleteAsync(Guid id);
    }
} 