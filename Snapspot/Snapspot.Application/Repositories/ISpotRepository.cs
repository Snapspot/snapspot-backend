using Snapspot.Application.Models.Styles;
using Snapspot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Application.Repositories
{
    public interface ISpotRepository
    {
        Task<Spot> GetByIdAsync(Guid id);
        Task<IEnumerable<Spot>> GetAllAsync();
        Task<IEnumerable<Spot>> GetByDistrictIdAsync(Guid districtId);
        Task AddAsync(Spot spot);
        Task UpdateAsync(Spot spot);
        Task DeleteAsync(Spot spot);
        Task<bool> ExistsAsync(Guid id);
        Task SaveChangesAsync();
       
        Task<IEnumerable<StyleDto>> GetStylesBySpotIdAsync(Guid spotId);
        Task<bool> AssignStyleToSpotAsync(Guid styleId, Guid spotId);
        Task<bool> RemoveStyleFromSpotIdAsync(Guid styleId, Guid spotId);
    }
} 