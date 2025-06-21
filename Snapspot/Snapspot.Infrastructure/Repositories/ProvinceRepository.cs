using Microsoft.EntityFrameworkCore;
using Snapspot.Application.Repositories;
using Snapspot.Domain.Entities;
using Snapspot.Infrastructure.Persistence.DBContext;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Infrastructure.Repositories
{
    public class ProvinceRepository : IProvinceRepository
    {
        private readonly AppDbContext _context;

        public ProvinceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Province> GetByIdAsync(Guid id)
        {
            return await _context.Set<Province>()
                .Include(p => p.Districts)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task<IEnumerable<Province>> GetAllAsync()
        {
            return await _context.Set<Province>()
                .Include(p => p.Districts)
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }

        public async Task AddAsync(Province province)
        {
            await _context.Set<Province>().AddAsync(province);
        }

        public Task UpdateAsync(Province province)
        {
            _context.Entry(province).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Province province)
        {
            province.IsDeleted = true;
            province.UpdatedAt = DateTime.UtcNow;
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Set<Province>().AnyAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
} 