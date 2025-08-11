using Microsoft.EntityFrameworkCore;
using Snapspot.Application.Models.Styles;
using Snapspot.Application.Repositories;
using Snapspot.Domain.Entities;
using Snapspot.Infrastructure.Persistence.DBContext;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Infrastructure.Repositories
{
    public class SpotRepository : ISpotRepository
    {
        private readonly AppDbContext _context;

        public SpotRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Spot> GetByIdAsync(Guid id)
        {
            return await _context.Set<Spot>()
                .Include(s => s.District)
                    .ThenInclude(d => d.Province)
                .Include(s => s.Agencies)
                    .ThenInclude(a => a.Company)
                .Include(s => s.Agencies)
                    .ThenInclude(a => a.Services)
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        }

        public async Task<IEnumerable<Spot>> GetAllAsync()
        {
            return await _context.Set<Spot>()
                .Include(s => s.District)
                    .ThenInclude(d => d.Province)
                .Include(s => s.Agencies)
                    .ThenInclude(a => a.Company)
                .Include(s => s.Agencies)
                    .ThenInclude(a => a.Services)
                .Where(s => !s.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Spot>> GetByDistrictIdAsync(Guid districtId)
        {
            return await _context.Set<Spot>()
                .Include(s => s.District)
                    .ThenInclude(d => d.Province)
                .Include(s => s.Agencies)
                    .ThenInclude(a => a.Company)
                .Include(s => s.Agencies)
                    .ThenInclude(a => a.Services)
                .Where(s => s.DistrictId == districtId && !s.IsDeleted)
                .ToListAsync();
        }

        public async Task AddAsync(Spot spot)
        {
            await _context.Set<Spot>().AddAsync(spot);
        }

        public Task UpdateAsync(Spot spot)
        {
            _context.Entry(spot).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Spot spot)
        {
            spot.IsDeleted = true;
            spot.UpdatedAt = DateTime.UtcNow;
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Set<Spot>().AnyAsync(s => s.Id == id && !s.IsDeleted);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        
      

        public async Task<bool> AssignStyleToSpotAsync(Guid styleId, Guid spotId)
        {
            var existingRelation = await _context.Set<StyleSpot>()
                .FirstOrDefaultAsync(ss => ss.StyleId == styleId && ss.SpotId == spotId && !ss.IsDeleted);

            if (existingRelation != null)
                return false;

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

        public async Task<bool> RemoveStyleFromSpotIdAsync(Guid styleId, Guid spotId)
        {
            var relation = await _context.Set<StyleSpot>()
                .FirstOrDefaultAsync(ss => ss.StyleId == styleId && ss.SpotId == spotId && !ss.IsDeleted);

            if (relation == null)
                return false;

            relation.IsDeleted = true;
            relation.UpdatedAt = DateTime.UtcNow;
            return true;
        }

        public async Task<IEnumerable<StyleDto>> GetStylesBySpotIdAsync(Guid spotId)
        {
            var styles = await _context.Set<StyleSpot>()
                .Include(ss => ss.Style)
                .Where(ss => ss.SpotId == spotId && !ss.IsDeleted && !ss.Style.IsDeleted)
                .Select(ss => new StyleDto
                {
                    Id = ss.Style.Id,
                    Category = ss.Style.Category,
                    Description = ss.Style.Description,
                    Image = ss.Style.Image,
                    CreatedAt = ss.Style.CreatedAt,
                    UpdatedAt = ss.Style.UpdatedAt,
                    IsDeleted = ss.Style.IsDeleted
                })
                .ToListAsync();

            return styles;
        }
    }
} 