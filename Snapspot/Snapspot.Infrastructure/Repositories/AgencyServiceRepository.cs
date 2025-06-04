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
    public class AgencyServiceRepository : IAgencyServiceRepository
    {
        private readonly AppDbContext _context;

        public AgencyServiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AgencyService> GetByIdAsync(Guid id)
        {
            return await _context.Set<AgencyService>()
                .Include(s => s.Agencies)
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        }

        public async Task<IEnumerable<AgencyService>> GetAllAsync()
        {
            return await _context.Set<AgencyService>()
                .Include(s => s.Agencies)
                .Where(s => !s.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<AgencyService>> GetByAgencyIdAsync(Guid agencyId)
        {
            return await _context.Set<AgencyService>()
                .Include(s => s.Agencies)
                .Where(s => s.Agencies.Any(a => a.Id == agencyId) && !s.IsDeleted)
                .ToListAsync();
        }

        public async Task AddAsync(AgencyService service)
        {
            await _context.Set<AgencyService>().AddAsync(service);
        }

        public Task UpdateAsync(AgencyService service)
        {
            _context.Entry(service).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(AgencyService service)
        {
            service.IsDeleted = true;
            service.UpdatedAt = DateTime.UtcNow;
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Set<AgencyService>().AnyAsync(s => s.Id == id && !s.IsDeleted);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddToAgencyAsync(Guid serviceId, Guid agencyId)
        {
            var service = await GetByIdAsync(serviceId);
            var agency = await _context.Agencies.FindAsync(agencyId);

            if (service != null && agency != null)
            {
                service.Agencies.Add(agency);
            }
        }

        public async Task RemoveFromAgencyAsync(Guid serviceId, Guid agencyId)
        {
            var service = await GetByIdAsync(serviceId);
            var agency = await _context.Agencies.FindAsync(agencyId);

            if (service != null && agency != null)
            {
                service.Agencies.Remove(agency);
            }
        }
    }
} 