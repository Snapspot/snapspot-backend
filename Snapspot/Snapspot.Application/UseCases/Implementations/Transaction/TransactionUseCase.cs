using Microsoft.EntityFrameworkCore;
using Snapspot.Application.DTOs.Transaction;
using Snapspot.Application.UseCases.Interfaces.Transaction;
using Snapspot.Domain.Entities;
using Snapspot.Shared.Common;
using Snapspot.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionEntity = Snapspot.Domain.Entities.Transaction;

namespace Snapspot.Application.UseCases.Implementations.Transaction
{
    //public class TransactionUseCase : ITransactionUseCase
    //{
    //    private readonly IAppDbContext _context;

    //    public TransactionUseCase(IAppDbContext context)
    //    {
    //        _context = context;
    //    }

    //    public async Task<ApiResponse<TransactionDTO>> CreateAsync(CreateTransactionDTO createTransactionDTO)
    //    {
    //        try
    //        {
    //            // Validate SellerPackage exists
    //            var sellerPackage = await _context.SellerPackages
    //                .FirstOrDefaultAsync(sp => sp.Id == createTransactionDTO.SellerPackageId && !sp.IsDeleted);
    //            if (sellerPackage == null)
    //            {
    //                return new ApiResponse<TransactionDTO>
    //                {
    //                    Success = false,
    //                    MessageId = MessageId.E0000,
    //                    Message = "SellerPackage not found"
    //                };
    //            }

    //            // Validate Company exists
    //            var company = await _context.Companies
    //                .FirstOrDefaultAsync(c => c.Id == createTransactionDTO.CompanyId && !c.IsDeleted);
    //            if (company == null)
    //            {
    //                return new ApiResponse<TransactionDTO>
    //                {
    //                    Success = false,
    //                    MessageId = MessageId.E0000,
    //                    Message = "Company not found"
    //                };
    //            }

    //            // Business logic: Check if company has sufficient balance
    //            if (createTransactionDTO.Amount > 1000000) // Example business rule
    //            {
    //                return new ApiResponse<TransactionDTO>
    //                {
    //                    Success = false,
    //                    MessageId = MessageId.E0000,
    //                    Message = "Transaction amount exceeds limit"
    //                };
    //            }

    //            var transaction = new TransactionEntity
    //            {
    //                Id = Guid.NewGuid(),
    //                SellerPackageId = createTransactionDTO.SellerPackageId,
    //                CompanyId = createTransactionDTO.CompanyId,
    //                TransactionCode = GenerateTransactionCode(),
    //                Amount = createTransactionDTO.Amount,
    //                PaymentType = createTransactionDTO.PaymentType,
    //                CreatedAt = DateTime.UtcNow,
    //                UpdatedAt = DateTime.UtcNow,
    //                IsDeleted = false
    //            };

    //            await _context.Transactions.AddAsync(transaction);
    //            await _context.SaveChangesAsync();

    //            var transactionDTO = new TransactionDTO
    //            {
    //                Id = transaction.Id,
    //                SellerPackageId = transaction.SellerPackageId,
    //                SellerPackageName = sellerPackage.Name,
    //                CompanyId = transaction.CompanyId,
    //                CompanyName = company.Name,
    //                TransactionCode = transaction.TransactionCode,
    //                Amount = transaction.Amount,
    //                PaymentType = transaction.PaymentType,
    //                CreatedAt = transaction.CreatedAt,
    //                IsDeleted = transaction.IsDeleted
    //            };

    //            return new ApiResponse<TransactionDTO>
    //            {
    //                Data = transactionDTO,
    //                Success = true,
    //                MessageId = MessageId.I0000,
    //                Message = Message.GetMessageById(MessageId.I0000)
    //            };
    //        }
    //        catch (Exception ex)
    //        {
    //            return new ApiResponse<TransactionDTO>
    //            {
    //                Success = false,
    //                MessageId = MessageId.E0000,
    //                Message = $"An error occurred while creating the transaction: {ex.Message}"
    //            };
    //        }
    //    }

    //    public async Task<ApiResponse<IEnumerable<TransactionDTO>>> GetAllAsync()
    //    {
    //        try
    //        {
    //            var transactions = await _context.Transactions
    //                .Include(t => t.SellerPackage)
    //                .Include(t => t.Company)
    //                .Where(t => !t.IsDeleted)
    //                .Select(t => new TransactionDTO
    //                {
    //                    Id = t.Id,
    //                    SellerPackageId = t.SellerPackageId,
    //                    SellerPackageName = t.SellerPackage.Name,
    //                    CompanyId = t.CompanyId,
    //                    CompanyName = t.Company.Name,
    //                    TransactionCode = t.TransactionCode,
    //                    Amount = t.Amount,
    //                    PaymentType = t.PaymentType,
    //                    CreatedAt = t.CreatedAt,
    //                    IsDeleted = t.IsDeleted
    //                })
    //                .ToListAsync();

    //            return new ApiResponse<IEnumerable<TransactionDTO>>
    //            {
    //                Data = transactions,
    //                Success = true,
    //                MessageId = MessageId.I0000,
    //                Message = Message.GetMessageById(MessageId.I0000)
    //            };
    //        }
    //        catch (Exception ex)
    //        {
    //            return new ApiResponse<IEnumerable<TransactionDTO>>
    //            {
    //                Success = false,
    //                MessageId = MessageId.E0000,
    //                Message = $"An error occurred while retrieving transactions: {ex.Message}"
    //            };
    //        }
    //    }

    //    public async Task<ApiResponse<TransactionDTO>> GetByIdAsync(Guid id)
    //    {
    //        try
    //        {
    //            var transaction = await _context.Transactions
    //                .Include(t => t.SellerPackage)
    //                .Include(t => t.Company)
    //                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

    //            if (transaction == null)
    //            {
    //                return new ApiResponse<TransactionDTO>
    //                {
    //                    Success = false,
    //                    MessageId = MessageId.E0000,
    //                    Message = "Transaction not found"
    //                };
    //            }

    //            var transactionDTO = new TransactionDTO
    //            {
    //                Id = transaction.Id,
    //                SellerPackageId = transaction.SellerPackageId,
    //                SellerPackageName = transaction.SellerPackage.Name,
    //                CompanyId = transaction.CompanyId,
    //                CompanyName = transaction.Company.Name,
    //                TransactionCode = transaction.TransactionCode,
    //                Amount = transaction.Amount,
    //                PaymentType = transaction.PaymentType,
    //                CreatedAt = transaction.CreatedAt,
    //                IsDeleted = transaction.IsDeleted
    //            };

    //            return new ApiResponse<TransactionDTO>
    //            {
    //                Data = transactionDTO,
    //                Success = true,
    //                MessageId = MessageId.I0000,
    //                Message = Message.GetMessageById(MessageId.I0000)
    //            };
    //        }
    //        catch (Exception ex)
    //        {
    //            return new ApiResponse<TransactionDTO>
    //            {
    //                Success = false,
    //                MessageId = MessageId.E0000,
    //                Message = $"An error occurred while retrieving the transaction: {ex.Message}"
    //            };
    //        }
    //    }

    //    public async Task<ApiResponse<IEnumerable<TransactionDTO>>> GetByCompanyIdAsync(Guid companyId)
    //    {
    //        try
    //        {
    //            var transactions = await _context.Transactions
    //                .Include(t => t.SellerPackage)
    //                .Include(t => t.Company)
    //                .Where(t => t.CompanyId == companyId && !t.IsDeleted)
    //                .Select(t => new TransactionDTO
    //                {
    //                    Id = t.Id,
    //                    SellerPackageId = t.SellerPackageId,
    //                    SellerPackageName = t.SellerPackage.Name,
    //                    CompanyId = t.CompanyId,
    //                    CompanyName = t.Company.Name,
    //                    TransactionCode = t.TransactionCode,
    //                    Amount = t.Amount,
    //                    PaymentType = t.PaymentType,
    //                    CreatedAt = t.CreatedAt,
    //                    IsDeleted = t.IsDeleted
    //                })
    //                .ToListAsync();

    //            return new ApiResponse<IEnumerable<TransactionDTO>>
    //            {
    //                Data = transactions,
    //                Success = true,
    //                MessageId = MessageId.I0000,
    //                Message = Message.GetMessageById(MessageId.I0000)
    //            };
    //        }
    //        catch (Exception ex)
    //        {
    //            return new ApiResponse<IEnumerable<TransactionDTO>>
    //            {
    //                Success = false,
    //                MessageId = MessageId.E0000,
    //                Message = $"An error occurred while retrieving transactions: {ex.Message}"
    //            };
    //        }
    //    }

    //    public async Task<ApiResponse<IEnumerable<TransactionDTO>>> GetBySellerPackageIdAsync(Guid sellerPackageId)
    //    {
    //        try
    //        {
    //            var transactions = await _context.Transactions
    //                .Include(t => t.SellerPackage)
    //                .Include(t => t.Company)
    //                .Where(t => t.SellerPackageId == sellerPackageId && !t.IsDeleted)
    //                .Select(t => new TransactionDTO
    //                {
    //                    Id = t.Id,
    //                    SellerPackageId = t.SellerPackageId,
    //                    SellerPackageName = t.SellerPackage.Name,
    //                    CompanyId = t.CompanyId,
    //                    CompanyName = t.Company.Name,
    //                    TransactionCode = t.TransactionCode,
    //                    Amount = t.Amount,
    //                    PaymentType = t.PaymentType,
    //                    CreatedAt = t.CreatedAt,
    //                    IsDeleted = t.IsDeleted
    //                })
    //                .ToListAsync();

    //            return new ApiResponse<IEnumerable<TransactionDTO>>
    //            {
    //                Data = transactions,
    //                Success = true,
    //                MessageId = MessageId.I0000,
    //                Message = Message.GetMessageById(MessageId.I0000)
    //            };
    //        }
    //        catch (Exception ex)
    //        {
    //            return new ApiResponse<IEnumerable<TransactionDTO>>
    //            {
    //                Success = false,
    //                MessageId = MessageId.E0000,
    //                Message = $"An error occurred while retrieving transactions: {ex.Message}"
    //            };
    //        }
    //    }

    //    public async Task<ApiResponse<IEnumerable<TransactionDTO>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    //    {
    //        try
    //        {
    //            var transactions = await _context.Transactions
    //                .Include(t => t.SellerPackage)
    //                .Include(t => t.Company)
    //                .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate && !t.IsDeleted)
    //                .Select(t => new TransactionDTO
    //                {
    //                    Id = t.Id,
    //                    SellerPackageId = t.SellerPackageId,
    //                    SellerPackageName = t.SellerPackage.Name,
    //                    CompanyId = t.CompanyId,
    //                    CompanyName = t.Company.Name,
    //                    TransactionCode = t.TransactionCode,
    //                    Amount = t.Amount,
    //                    PaymentType = t.PaymentType,
    //                    CreatedAt = t.CreatedAt,
    //                    IsDeleted = t.IsDeleted
    //                })
    //                .ToListAsync();

    //            return new ApiResponse<IEnumerable<TransactionDTO>>
    //            {
    //                Data = transactions,
    //                Success = true,
    //                MessageId = MessageId.I0000,
    //                Message = Message.GetMessageById(MessageId.I0000)
    //            };
    //        }
    //        catch (Exception ex)
    //        {
    //            return new ApiResponse<IEnumerable<TransactionDTO>>
    //            {
    //                Success = false,
    //                MessageId = MessageId.E0000,
    //                Message = $"An error occurred while retrieving transactions: {ex.Message}"
    //            };
    //        }
    //    }

    //    public async Task<ApiResponse<decimal>> GetTotalRevenueAsync(Guid companyId)
    //    {
    //        try
    //        {
    //            var totalRevenue = await _context.Transactions
    //                .Where(t => t.CompanyId == companyId && !t.IsDeleted)
    //                .SumAsync(t => t.Amount);

    //            return new ApiResponse<decimal>
    //            {
    //                Data = totalRevenue,
    //                Success = true,
    //                MessageId = MessageId.I0000,
    //                Message = Message.GetMessageById(MessageId.I0000)
    //            };
    //        }
    //        catch (Exception ex)
    //        {
    //            return new ApiResponse<decimal>
    //            {
    //                Success = false,
    //                MessageId = MessageId.E0000,
    //                Message = $"An error occurred while calculating revenue: {ex.Message}"
    //            };
    //        }
    //    }

    //    public async Task<ApiResponse<string>> CancelTransactionAsync(Guid id)
    //    {
    //        try
    //        {
    //            var transaction = await _context.Transactions
    //                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

    //            if (transaction == null)
    //            {
    //                return new ApiResponse<string>
    //                {
    //                    Success = false,
    //                    MessageId = MessageId.E0000,
    //                    Message = "Transaction not found"
    //                };
    //            }

    //            // Business logic: Check if transaction can be cancelled (within 24 hours)
    //            if (DateTime.UtcNow.Subtract(transaction.CreatedAt).TotalHours > 24)
    //            {
    //                return new ApiResponse<string>
    //                {
    //                    Success = false,
    //                    MessageId = MessageId.E0000,
    //                    Message = "Transaction cannot be cancelled after 24 hours"
    //                };
    //            }

    //            transaction.IsDeleted = true;
    //            transaction.UpdatedAt = DateTime.UtcNow;
    //            await _context.SaveChangesAsync();

    //            return new ApiResponse<string>
    //            {
    //                Success = true,
    //                MessageId = MessageId.I0000,
    //                Message = "Transaction cancelled successfully"
    //            };
    //        }
    //        catch (Exception ex)
    //        {
    //            return new ApiResponse<string>
    //            {
    //                Success = false,
    //                MessageId = MessageId.E0000,
    //                Message = $"An error occurred while cancelling the transaction: {ex.Message}"
    //            };
    //        }
    //    }

    //    private string GenerateTransactionCode()
    //    {
    //        return $"TXN{DateTime.UtcNow:yyyyMMddHHmmss}{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
    //    }
    //}
} 