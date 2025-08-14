using Snapspot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Application.Repositories
{
    public interface ITransactionRepository : IGenericRepository<Transaction, Guid>
    {
        Task<IEnumerable<Transaction>> GetByCompanyIdAsync(Guid companyId);
        Task<IEnumerable<Transaction>> GetBySellerPackageIdAsync(Guid sellerPackageId);
        Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalRevenueAsync(Guid companyId);
        Task<Transaction> AddAsync(Transaction entity);
        Task<Transaction?> GetByTransactionCode(string TransactionCode);
        Task<decimal> GetTotalAmountInCurrentMonth();
    }
} 