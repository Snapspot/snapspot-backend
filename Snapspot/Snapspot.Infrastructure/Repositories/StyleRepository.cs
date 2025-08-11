using Microsoft.EntityFrameworkCore;
using Snapspot.Application.Repositories;
using Snapspot.Domain.Entities;
using Snapspot.Infrastructure.Persistence.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Infrastructure.Repositories
{
    public class StyleRepository : IStyleRepository
    {
        private readonly AppDbContext _context;

        public StyleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Style> GetByIdAsync(Guid id)
        {
            return await _context.Set<Style>()
                .Include(s => s.StyleSpots)
                    .ThenInclude(ss => ss.Spot)
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        }

        public async Task<IEnumerable<Style>> GetAllAsync()
        {
            return await _context.Set<Style>()
                .Include(s => s.StyleSpots)
                .Where(s => !s.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Style>> GetByCategoryAsync(string category)
        {
            return await _context.Set<Style>()
                .Include(s => s.StyleSpots)
                .Where(s => s.Category.Contains(category) && !s.IsDeleted)
                .ToListAsync();
        }

        public async Task AddAsync(Style style)
        {
            await _context.Set<Style>().AddAsync(style);
        }

        public Task UpdateAsync(Style style)
        {
            _context.Entry(style).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Style style)
        {
            style.IsDeleted = true;
            style.UpdatedAt = DateTime.UtcNow;
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Set<Style>().AnyAsync(s => s.Id == id && !s.IsDeleted);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        // Methods cho mối quan hệ Style-Spot
        public async Task<bool> AssignStyleToSpotAsync(Guid styleId, Guid spotId)
        {
            var existingRelation = await _context.Set<StyleSpot>()
                .FirstOrDefaultAsync(ss => ss.StyleId == styleId && ss.SpotId == spotId && !ss.IsDeleted);

            if (existingRelation != null)
                return false; // Đã tồn tại

            var styleSpot = new StyleSpot
            {
                StyleId = styleId,
                SpotId = spotId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.Set<StyleSpot>().AddAsync(styleSpot);
            return true;
        }

        public async Task<bool> RemoveStyleFromSpotAsync(Guid styleId, Guid spotId)
        {
            var relation = await _context.Set<StyleSpot>()
                .FirstOrDefaultAsync(ss => ss.StyleId == styleId && ss.SpotId == spotId && !ss.IsDeleted);

            if (relation == null)
                return false; // Không tồn tại

            relation.IsDeleted = true;
            relation.UpdatedAt = DateTime.UtcNow;
            return true;
        }

        public async Task<IEnumerable<Style>> GetStylesBySpotIdAsync(Guid spotId)
        {
            return await _context.Set<StyleSpot>()
                .Include(ss => ss.Style)
                .Where(ss => ss.SpotId == spotId && !ss.IsDeleted && !ss.Style.IsDeleted)
                .Select(ss => ss.Style)
                .ToListAsync();
        }

        public async Task<IEnumerable<Spot>> GetSpotsByStyleIdAsync(Guid styleId)
        {
            return await _context.Set<StyleSpot>()
                .Include(ss => ss.Spot)
                .Where(ss => ss.StyleId == styleId && !ss.IsDeleted && !ss.Spot.IsDeleted)
                .Select(ss => ss.Spot)
                .ToListAsync();
        }
    }
}
