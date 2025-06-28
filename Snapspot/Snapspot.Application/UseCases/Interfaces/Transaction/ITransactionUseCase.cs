using Snapspot.Application.DTOs.Transaction;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Application.UseCases.Interfaces.Transaction
{
    public interface ITransactionUseCase
    {
        // CRUD Operations
        Task<ApiResponse<TransactionDTO>> GetByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<TransactionDTO>>> GetAllAsync();
        Task<ApiResponse<TransactionDTO>> CreateAsync(CreateTransactionDTO createTransactionDTO);
        
        // Business Operations
        Task<ApiResponse<IEnumerable<TransactionDTO>>> GetByCompanyIdAsync(Guid companyId);
        Task<ApiResponse<IEnumerable<TransactionDTO>>> GetBySellerPackageIdAsync(Guid sellerPackageId);
        Task<ApiResponse<IEnumerable<TransactionDTO>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<ApiResponse<decimal>> GetTotalRevenueAsync(Guid companyId);
        Task<ApiResponse<string>> CancelTransactionAsync(Guid id);
    }
} 