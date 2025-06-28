using Snapspot.Application.Models.SellerPackages;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionEntity = Snapspot.Domain.Entities.Transaction;

namespace Snapspot.Application.UseCases.Interfaces.SellerPackage
{
    public interface ISellerPackageUseCase
    {
        Task<ApiResponse<SellerPackageDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<SellerPackageDto>>> GetAllAsync();
        Task<ApiResponse<SellerPackageDto>> CreateAsync(CreateSellerPackageDto createSellerPackageDto);
        Task<ApiResponse<SellerPackageDto>> UpdateAsync(Guid id, UpdateSellerPackageDto updateSellerPackageDto);
        Task<ApiResponse<string>> DeleteAsync(Guid id);
        Task<ApiResponse<bool>> ExistsAsync(Guid id);
    }
} 