using Snapspot.Application.DTOs.Transaction;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<Result<TransactionDTO>> CreateTransactionAsync(CreateTransactionDTO createTransactionDTO);
        Task<Result<List<TransactionDTO>>> GetAllTransactionsAsync();
        Task<Result<List<TransactionDTO>>> GetTransactionsByCompanyIdAsync(Guid companyId);
        Task<Result<TransactionDTO>> GetTransactionByIdAsync(Guid id);
    }
} 