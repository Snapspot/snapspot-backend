using Snapspot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Application.Repositories
{
    public interface ISellerPackageRepository
    {
        Task<SellerPackage> GetByIdAsync(Guid id);
        Task<IEnumerable<SellerPackage>> GetAllAsync();
        Task AddAsync(SellerPackage sellerPackage);
        Task UpdateAsync(SellerPackage sellerPackage);
        Task DeleteAsync(SellerPackage sellerPackage);
        Task<bool> ExistsAsync(Guid id);
        Task SaveChangesAsync();
    }
} 