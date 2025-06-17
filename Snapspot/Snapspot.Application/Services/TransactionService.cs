using Microsoft.EntityFrameworkCore;
using Snapspot.Application.DTOs.Transaction;
using Snapspot.Application.Interfaces;
using Snapspot.Domain.Entities;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snapspot.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IAppDbContext _context;

        public TransactionService(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<TransactionDTO>> CreateTransactionAsync(CreateTransactionDTO createTransactionDTO)
        {
            try
            {
                // Validate SellerPackage exists
                var sellerPackage = await _context.SellerPackages
                    .FirstOrDefaultAsync(sp => sp.Id == createTransactionDTO.SellerPackageId && !sp.IsDeleted);
                if (sellerPackage == null)
                    return Result<TransactionDTO>.Failure("SellerPackage not found");

                // Validate Company exists
                var company = await _context.Companies
                    .FirstOrDefaultAsync(c => c.Id == createTransactionDTO.CompanyId && !c.IsDeleted);
                if (company == null)
                    return Result<TransactionDTO>.Failure("Company not found");

                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    SellerPackageId = createTransactionDTO.SellerPackageId,
                    CompanyId = createTransactionDTO.CompanyId,
                    TransactionCode = createTransactionDTO.TransactionCode,
                    Amount = createTransactionDTO.Amount,
                    PaymentType = createTransactionDTO.PaymentType,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                await _context.Transactions.AddAsync(transaction);
                await _context.SaveChangesAsync();

                return Result<TransactionDTO>.Success(new TransactionDTO
                {
                    Id = transaction.Id,
                    SellerPackageId = transaction.SellerPackageId,
                    SellerPackageName = sellerPackage.Name,
                    CompanyId = transaction.CompanyId,
                    CompanyName = company.Name,
                    TransactionCode = transaction.TransactionCode,
                    Amount = transaction.Amount,
                    PaymentType = transaction.PaymentType,
                    CreatedAt = transaction.CreatedAt,
                    IsDeleted = transaction.IsDeleted
                });
            }
            catch (Exception ex)
            {
                return Result<TransactionDTO>.Failure($"An error occurred while creating the transaction: {ex.Message}");
            }
        }

        public async Task<Result<List<TransactionDTO>>> GetAllTransactionsAsync()
        {
            try
            {
                var transactions = await _context.Transactions
                    .Include(t => t.SellerPackage)
                    .Include(t => t.Company)
                    .Where(t => !t.IsDeleted)
                    .Select(t => new TransactionDTO
                    {
                        Id = t.Id,
                        SellerPackageId = t.SellerPackageId,
                        SellerPackageName = t.SellerPackage.Name,
                        CompanyId = t.CompanyId,
                        CompanyName = t.Company.Name,
                        TransactionCode = t.TransactionCode,
                        Amount = t.Amount,
                        PaymentType = t.PaymentType,
                        CreatedAt = t.CreatedAt,
                        IsDeleted = t.IsDeleted
                    })
                    .ToListAsync();

                return Result<List<TransactionDTO>>.Success(transactions);
            }
            catch (Exception ex)
            {
                return Result<List<TransactionDTO>>.Failure($"An error occurred while retrieving transactions: {ex.Message}");
            }
        }

        public async Task<Result<List<TransactionDTO>>> GetTransactionsByCompanyIdAsync(Guid companyId)
        {
            try
            {
                var transactions = await _context.Transactions
                    .Include(t => t.SellerPackage)
                    .Include(t => t.Company)
                    .Where(t => t.CompanyId == companyId && !t.IsDeleted)
                    .Select(t => new TransactionDTO
                    {
                        Id = t.Id,
                        SellerPackageId = t.SellerPackageId,
                        SellerPackageName = t.SellerPackage.Name,
                        CompanyId = t.CompanyId,
                        CompanyName = t.Company.Name,
                        TransactionCode = t.TransactionCode,
                        Amount = t.Amount,
                        PaymentType = t.PaymentType,
                        CreatedAt = t.CreatedAt,
                        IsDeleted = t.IsDeleted
                    })
                    .ToListAsync();

                return Result<List<TransactionDTO>>.Success(transactions);
            }
            catch (Exception ex)
            {
                return Result<List<TransactionDTO>>.Failure($"An error occurred while retrieving transactions: {ex.Message}");
            }
        }

        public async Task<Result<TransactionDTO>> GetTransactionByIdAsync(Guid id)
        {
            try
            {
                var transaction = await _context.Transactions
                    .Include(t => t.SellerPackage)
                    .Include(t => t.Company)
                    .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

                if (transaction == null)
                    return Result<TransactionDTO>.Failure("Transaction not found");

                var transactionDTO = new TransactionDTO
                {
                    Id = transaction.Id,
                    SellerPackageId = transaction.SellerPackageId,
                    SellerPackageName = transaction.SellerPackage.Name,
                    CompanyId = transaction.CompanyId,
                    CompanyName = transaction.Company.Name,
                    TransactionCode = transaction.TransactionCode,
                    Amount = transaction.Amount,
                    PaymentType = transaction.PaymentType,
                    CreatedAt = transaction.CreatedAt,
                    IsDeleted = transaction.IsDeleted
                };

                return Result<TransactionDTO>.Success(transactionDTO);
            }
            catch (Exception ex)
            {
                return Result<TransactionDTO>.Failure($"An error occurred while retrieving the transaction: {ex.Message}");
            }
        }
    }
} 