using Snapspot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Application.Repositories
{
    public interface IProvinceRepository
    {
        Task<Province> GetByIdAsync(Guid id);
        Task<IEnumerable<Province>> GetAllAsync();
        Task AddAsync(Province province);
        Task UpdateAsync(Province province);
        Task DeleteAsync(Province province);
        Task<bool> ExistsAsync(Guid id);
        Task SaveChangesAsync();
    }
} 