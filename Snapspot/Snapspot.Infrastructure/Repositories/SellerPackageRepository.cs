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
    public class SellerPackageRepository : ISellerPackageRepository
    {
        private readonly AppDbContext _context;

        public SellerPackageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SellerPackage> GetByIdAsync(Guid id)
        {
            return await _context.Set<SellerPackage>()
                .Include(sp => sp.Companies)
                .FirstOrDefaultAsync(sp => sp.Id == id && !sp.IsDeleted);
        }

        public async Task<IEnumerable<SellerPackage>> GetAllAsync()
        {
            return await _context.Set<SellerPackage>()
                .Include(sp => sp.Companies)
                .Where(sp => !sp.IsDeleted)
                .ToListAsync();
        }

        public async Task AddAsync(SellerPackage sellerPackage)
        {
            await _context.Set<SellerPackage>().AddAsync(sellerPackage);
        }

        public Task UpdateAsync(SellerPackage sellerPackage)
        {
            _context.Entry(sellerPackage).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(SellerPackage sellerPackage)
        {
            sellerPackage.IsDeleted = true;
            sellerPackage.UpdatedAt = DateTime.UtcNow;
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Set<SellerPackage>().AnyAsync(sp => sp.Id == id && !sp.IsDeleted);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
} 