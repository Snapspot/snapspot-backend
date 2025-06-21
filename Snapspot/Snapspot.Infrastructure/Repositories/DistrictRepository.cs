using Microsoft.EntityFrameworkCore;
using Snapspot.Application.Repositories;
using Snapspot.Domain.Entities;
using Snapspot.Infrastructure.Persistence.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snapspot.Infrastructure.Repositories
{
    public class DistrictRepository : IDistrictRepository
    {
        private readonly AppDbContext _context;

        public DistrictRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<District> GetByIdAsync(Guid id)
        {
            return await _context.Set<District>()
                .Include(d => d.Province)
                .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);
        }

        public async Task<IEnumerable<District>> GetAllAsync()
        {
            return await _context.Set<District>()
                .Include(d => d.Province)
                .Where(d => !d.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<District>> GetByProvinceIdAsync(Guid provinceId)
        {
            return await _context.Set<District>()
                .Include(d => d.Province)
                .Where(d => d.ProvinceId == provinceId && !d.IsDeleted)
                .ToListAsync();
        }

        public async Task AddAsync(District district)
        {
            await _context.Set<District>().AddAsync(district);
        }

        public Task UpdateAsync(District district)
        {
            _context.Entry(district).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(District district)
        {
            district.IsDeleted = true;
            district.UpdatedAt = DateTime.UtcNow;
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Set<District>().AnyAsync(d => d.Id == id && !d.IsDeleted);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
} 