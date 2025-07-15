using Microsoft.EntityFrameworkCore;
using Snapspot.Application.Repositories;
using Snapspot.Domain.Entities;
using Snapspot.Infrastructure.Persistence.DBContext;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Infrastructure.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDbContext _context;

        public CompanyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Company?> GetByIdAsync(Guid id)
        {
            return await _context.Set<Company>()
                .Include(c => c.User)
                .Include(c => c.Agencies)
                    .ThenInclude(a => a.Spot)
                .Include(c => c.Agencies)
                    .ThenInclude(a => a.Services)
                .Include(c => c.Agencies)
                    .ThenInclude(a => a.Feedbacks)
                        .ThenInclude(f => f.User)
                .Include(c => c.CompanySellerPackages)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _context.Set<Company>()
                .Include(c => c.User)
                .Include(c => c.Agencies)
                    .ThenInclude(a => a.Spot)
                .Include(c => c.Agencies)
                    .ThenInclude(a => a.Services)
                .Include(c => c.Agencies)
                    .ThenInclude(a => a.Feedbacks)
                        .ThenInclude(f => f.User)
                .Include(c => c.CompanySellerPackages)
                .Where(c => !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<Company?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Set<Company>()
                .Where(c => !c.IsDeleted && c.User != null && c.User.Id == userId)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(Company company)
        {
            await _context.Set<Company>().AddAsync(company);
        }

        public Task UpdateAsync(Company company)
        {
            _context.Entry(company).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Company company)
        {
            company.IsDeleted = true;
            company.UpdatedAt = DateTime.UtcNow;
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Set<Company>().AnyAsync(c => c.Id == id && !c.IsDeleted);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
} 