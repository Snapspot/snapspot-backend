using Snapspot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.Repositories
{
    public interface IStyleRepository
    {
        Task<Style> GetByIdAsync(Guid id);
        Task<IEnumerable<Style>> GetAllAsync();
        Task<IEnumerable<Style>> GetByCategoryAsync(string category);
        Task AddAsync(Style style);
        Task UpdateAsync(Style style);
        Task DeleteAsync(Style style);
        Task<bool> ExistsAsync(Guid id);
        Task SaveChangesAsync();

        // Methods cho mối quan hệ Style-Spot
        Task<bool> AssignStyleToSpotAsync(Guid styleId, Guid spotId);
        Task<bool> RemoveStyleFromSpotAsync(Guid styleId, Guid spotId);
        Task<IEnumerable<Style>> GetStylesBySpotIdAsync(Guid spotId);
        Task<IEnumerable<Spot>> GetSpotsByStyleIdAsync(Guid styleId);
    }
}
