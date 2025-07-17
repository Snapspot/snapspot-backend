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
    public class AgencyRepository : IAgencyRepository
    {
        private readonly AppDbContext _context;

        public AgencyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Agency> GetByIdAsync(Guid id)
        {
            return await _context.Agencies
                .Include(a => a.Company)
                .Include(a => a.Spot)
                .Include(a => a.Services)
                .Include(a => a.Feedbacks)
                    .ThenInclude(f => f.User)
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
        }

        public async Task<IEnumerable<Agency>> GetAllAsync()
        {
            return await _context.Agencies
                .Include(a => a.Company)
                .Include(a => a.Spot)
                .Include(a => a.Services)
                .Include(a => a.Feedbacks)
                    .ThenInclude(f => f.User)
                .Where(a => !a.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Agency>> GetByCompanyIdAsync(Guid companyId)
        {
            return await _context.Agencies
                .Include(a => a.Company)
                .Include(a => a.Spot)
                .Include(a => a.Services)
                .Include(a => a.Feedbacks)
                    .ThenInclude(f => f.User)
                .Where(a => a.CompanyId == companyId && !a.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Agency>> GetBySpotIdAsync(Guid spotId)
        {
            return await _context.Agencies
                .Include(a => a.Company)
                .Include(a => a.Spot)
                .Include(a => a.Services)
                .Include(a => a.Feedbacks)
                    .ThenInclude(f => f.User)
                .Where(a => a.SpotId == spotId && !a.IsDeleted)
                .ToListAsync();
        }

        public async Task AddAsync(Agency agency)
        {
            await _context.Agencies.AddAsync(agency);
        }

        public Task UpdateAsync(Agency agency)
        {
            _context.Entry(agency).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Agency agency)
        {
            agency.IsDeleted = true;
            agency.UpdatedAt = DateTime.UtcNow;
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Agencies.AnyAsync(a => a.Id == id && !a.IsDeleted);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCurrentActiveAgency(Guid companyId)
        {
           return await _context.Agencies.CountAsync(a => a.CompanyId == companyId && !a.IsDeleted);
        }
    }
} 