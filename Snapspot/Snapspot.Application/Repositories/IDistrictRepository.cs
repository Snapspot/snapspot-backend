using Snapspot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Application.Repositories
{
    public interface IDistrictRepository
    {
        Task<District> GetByIdAsync(Guid id);
        Task<IEnumerable<District>> GetAllAsync();
        Task<IEnumerable<District>> GetByProvinceIdAsync(Guid provinceId);
        Task AddAsync(District district);
        Task UpdateAsync(District district);
        Task DeleteAsync(District district);
        Task<bool> ExistsAsync(Guid id);
        Task SaveChangesAsync();
    }
} 