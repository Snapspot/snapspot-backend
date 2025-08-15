using Microsoft.EntityFrameworkCore;
using Snapspot.Application.Repositories;
using Snapspot.Domain.Entities;
using Snapspot.Infrastructure.Persistence.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Snapspot.Application.Repositories.ISellerPackageRepository;

namespace Snapspot.Infrastructure.Repositories
{
    public class SellerPackageRepository : ISellerPackageRepository
    {
        private readonly AppDbContext _context;

        public SellerPackageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SellerPackage?> GetByIdAsync(Guid id)
        {
            return await _context.SellerPackages
                 .Include(sp => sp.CompanySellerPackages)
                 .ThenInclude(csp => csp.Company)
                 .FirstOrDefaultAsync(sp => sp.Id == id);
        }

        public async Task<IEnumerable<SellerPackage>> GetAllAsync()
        {
            return await _context.SellerPackages
                 .Include(sp => sp.CompanySellerPackages)
                 .ThenInclude(csp => csp.Company)
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

        public async Task<List<ISellerPackageRepository.PackageCoverageDto>> GetPackageSalesCoverageAsync()
        {
            var groupedTransactions = await _context.Transactions
                .GroupBy(t => t.SellerPackageId)
                .Select(g => new
                {
                    PackageId = g.Key,
                    SalesCount = g.Count()
                })
                .ToListAsync();

            var packageIds = groupedTransactions.Select(g => g.PackageId).ToList();

            var packages = await _context.SellerPackages
                .Where(p => packageIds.Contains(p.Id))
                .ToListAsync();

            var result = groupedTransactions.Select(gt => new PackageCoverageDto
            (
                Name: packages.FirstOrDefault(p => p.Id == gt.PackageId)?.Name ?? "Undefined",
                Value: gt.SalesCount
            )).ToList();

            return result;
        }

        public async Task<object> GetPackageRevenue()
        {
            // Step 1: Calculate the total amount for each package
            var groupedTransactions = await _context.Transactions
                .GroupBy(t => t.SellerPackageId)
                .Select(g => new
                {
                    PackageId = g.Key,
                    TotalAmount = g.Sum(t => t.Amount)
                })
                .ToListAsync();

            // Step 2: Get package names for the sold packages
            var packageIds = groupedTransactions.Select(g => g.PackageId).ToList();

            var packages = await _context.SellerPackages
                .Where(p => packageIds.Contains(p.Id))
                .ToListAsync();

            // Step 3: Construct the first part of the result (the revenue data object)
            var revenueData = new Dictionary<string, object>();
            revenueData["name"] = "Doanh thu";

            foreach (var transactionGroup in groupedTransactions)
            {
                revenueData[transactionGroup.PackageId.ToString()] = transactionGroup.TotalAmount;
            }

            // Step 4: Construct the second part of the result (the legend/mapping array)
            var packageMappings = packages.Select(p => new
            {
                dataKey = p.Id.ToString(),
                name = p.Name
            }).ToList();

            // Step 5: Combine both parts into a single response object
            var result = new
            {
                Revenue = revenueData,
                Packages = packageMappings
            };

            return result;
        }

    }
} 