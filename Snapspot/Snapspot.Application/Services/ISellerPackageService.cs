using Snapspot.Application.Models.SellerPackages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Application.Services
{
    public interface ISellerPackageService
    {
        Task<SellerPackageDto> GetByIdAsync(Guid id);
        Task<IEnumerable<SellerPackageDto>> GetAllAsync();
        Task<SellerPackageDto> CreateAsync(CreateSellerPackageDto createSellerPackageDto);
        Task<SellerPackageDto> UpdateAsync(Guid id, UpdateSellerPackageDto updateSellerPackageDto);
        Task<bool> DeleteAsync(Guid id);
    }
} 