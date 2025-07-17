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
    public class CompanySellerPackageRepository : ICompanySellerPackageRepository
    {
        private readonly AppDbContext _context;

        public CompanySellerPackageRepository(AppDbContext context)
        {
            _context = context;
        }
        public Task<IEnumerable<Guid>> GetCompaniesBySellerPackageAsync(Guid sellerPackageId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Guid>> GetSellerPackagesByCompanyAsync(Guid companyId)
        {
            throw new NotImplementedException();
        }

        public async Task<CompanySellerPackage?> CompanyInSellerPackageAsync(Guid companyId, Guid sellerPackageId)
        {
            return await _context.Set<CompanySellerPackage>()
                .FirstOrDefaultAsync(csp => csp.CompaniesId == companyId && csp.SellerPackagesId == sellerPackageId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Add(CompanySellerPackage companySellerPackage)
        {
            await _context.Set<CompanySellerPackage>().AddAsync(companySellerPackage);
        }

        public async Task<CompanySellerPackage?> GetSubcriptionHasHighestAgency(Guid companyId)
        {
            return await _context.Set<CompanySellerPackage>()
                .Where(x => x.CompaniesId == companyId && x.IsActive)
                .Include(x => x.SellerPackage)
                .OrderByDescending(x => x.SellerPackage.MaxAgency)
                .FirstOrDefaultAsync();
        }
    }
}
