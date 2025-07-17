using Snapspot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Application.Repositories
{
    public interface IAgencyRepository
    {
        Task<Agency> GetByIdAsync(Guid id);
        Task<IEnumerable<Agency>> GetAllAsync();
        Task<IEnumerable<Agency>> GetByCompanyIdAsync(Guid companyId);
        Task<IEnumerable<Agency>> GetBySpotIdAsync(Guid spotId);
        Task<int> GetCurrentActiveAgency(Guid companyId);
        Task AddAsync(Agency agency);
        Task UpdateAsync(Agency agency);
        Task DeleteAsync(Agency agency);
        Task<bool> ExistsAsync(Guid id);
        Task SaveChangesAsync();
    }
} 