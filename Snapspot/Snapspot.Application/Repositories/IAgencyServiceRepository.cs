using Snapspot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Application.Repositories
{
    public interface IAgencyServiceRepository
    {
        Task<AgencyService> GetByIdAsync(Guid id);
        Task<IEnumerable<AgencyService>> GetAllAsync();
        Task<IEnumerable<AgencyService>> GetByAgencyIdAsync(Guid agencyId);
        Task AddAsync(AgencyService service);
        Task UpdateAsync(AgencyService service);
        Task DeleteAsync(AgencyService service);
        Task<bool> ExistsAsync(Guid id);
        Task SaveChangesAsync();
        Task AddToAgencyAsync(Guid serviceId, Guid agencyId);
        Task RemoveFromAgencyAsync(Guid serviceId, Guid agencyId);
        Task<IEnumerable<AgencyService>> FindAllAsync(Guid[] expectedIds);
    }
} 