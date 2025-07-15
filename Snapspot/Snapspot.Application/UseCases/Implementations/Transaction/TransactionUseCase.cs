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
using Snapspot.Application.Services;
using Snapspot.Application.Repositories;
using Snapspot.Application.Models.Responses.Payment;
using Net.payOS;
using Net.payOS.Types;
using Snapspot.Application.Models.Requests.Payment;
using static System.Net.Mime.MediaTypeNames;

namespace Snapspot.Application.UseCases.Implementations.Transaction
{
    public class TransactionUseCase : ITransactionUseCase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ISellerPackageRepository _sellerPackageRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IPayOSService _payOSService;
        private readonly PayOS _payOS;
        private readonly ICompanySellerPackageRepository _companySellerPackageRepository;

        public TransactionUseCase(
            ITransactionRepository transactionRepository,
            ISellerPackageRepository sellerPackageRepository,
            ICompanyRepository companyRepository,
            IPayOSService payOSService,
            PayOS payos,
            ICompanySellerPackageRepository companySellerPackageRepository)
        {
            _transactionRepository = transactionRepository;
            _sellerPackageRepository = sellerPackageRepository;
            _companyRepository = companyRepository;
            _payOSService = payOSService;
            _payOS = payos;
            _companySellerPackageRepository = companySellerPackageRepository;
        }

        public async Task<ApiResponse<string>> CreateUserSubscription(WebhookData webhookData)
        {
            var transaction = await _transactionRepository.GetByTransactionCode(webhookData.orderCode.ToString());
            if (transaction == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "cannot find transaction"
                };
            }


            var subscription = await _companySellerPackageRepository.CompanyInSellerPackageAsync(transaction.Company.Id, transaction.SellerPackage.Id);

            int month = (int)transaction.Amount / (int)transaction.SellerPackage.Price;

            if (subscription != null)
            {
              
                subscription.RemainingDay += month * 30; // Assuming 30 days in a month
                return new ApiResponse<string>
                {
                    Success = true,
                    Message = "Remaining day has been extended for " + month + " months."
                };
            }
            else
            {
                var newSubcription = new CompanySellerPackage
                {
                    CompaniesId = transaction.Company.Id,
                    SellerPackagesId = transaction.SellerPackage.Id,
                    RemainingDay = (int)transaction.Amount / (int)transaction.SellerPackage.Price * 30, // Assuming 30 days in a month
                    IsActive = true
                };
                return new ApiResponse<string>
                {
                    Success = true,
                    Message = "Subcription has been created for " + month + " months."
                };
            }
        }

        public ApiResponse<WebhookData> VerifyPaymentWebhookData(WebhookType webHookType)
        {
            var response = _payOS.verifyPaymentWebhookData(webHookType);
            return ApiResponse<WebhookData>.Ok(response);
        }

        public async Task<ApiResponse<PaymentResponse>> CreatePaymentUrl(Guid userId, PaymentRequest request)
        {
           var company = await _companyRepository.GetByUserIdAsync(userId);
            if (company == null)
            {
                return new ApiResponse<PaymentResponse>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = "Company not found"
                };
            }
            var sellerPackage = await _sellerPackageRepository.GetByIdAsync(request.SellerPackageId);
            if (sellerPackage == null)
            {
                return new ApiResponse<PaymentResponse>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = "SellerPackage not found"
                };
            }

            ItemData item = new ItemData(sellerPackage.Name, request.Month, (int)sellerPackage.Price);
            List<ItemData> items = new List<ItemData>();
            items.Add(item);

            var cancelUrl = "https://snapspot.site/home";
            var returnUrl = "https://snapspot.site/home";

            TransactionEntity newTransaction = new()
            {
                SellerPackageId = request.SellerPackageId,
                CompanyId = company.Id,
                TransactionCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(), // Example transaction code, you might want to use a more complex logic
                Amount = (int)sellerPackage.Price * request.Month,
                PaymentType = "Chuyển khoản",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _transactionRepository.AddAsync(newTransaction);
            await _transactionRepository.SaveChangesAsync();

            PaymentData paymentData = new PaymentData(long.Parse(newTransaction.TransactionCode), (int)newTransaction.Amount, "Thanh toan don hang",
                 items, cancelUrl, returnUrl);

            CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

            var response = new ApiResponse<PaymentResponse>
            {
                Data = new PaymentResponse
                {
                    CheckoutUrl = createPayment.checkoutUrl,
                },
                Success = true,
                MessageId = MessageId.I0000,
                Message = Message.GetMessageById(MessageId.I0000)
            };

            return response;
        }

        public async Task<ApiResponse<TransactionDTO>> CreateAsync(CreateTransactionDTO createTransactionDTO)
        {
            try
            {
                // Validate SellerPackage exists
                var sellerPackage = await _sellerPackageRepository.GetByIdAsync(createTransactionDTO.SellerPackageId);
                if (sellerPackage == null)
                {
                    return new ApiResponse<TransactionDTO>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "SellerPackage not found"
                    };
                }

                // Validate Company exists
                var company = await _companyRepository.GetByIdAsync(createTransactionDTO.CompanyId);
                if (company == null)
                {
                    return new ApiResponse<TransactionDTO>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Company not found"
                    };
                }

                // Business logic: Check if company has sufficient balance
                if (createTransactionDTO.Amount > 1000000) // Example business rule
                {
                    return new ApiResponse<TransactionDTO>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Transaction amount exceeds limit"
                    };
                }

                var transaction = new TransactionEntity
                {
                    Id = Guid.NewGuid(),
                    SellerPackageId = createTransactionDTO.SellerPackageId,
                    CompanyId = createTransactionDTO.CompanyId,
                    TransactionCode = GenerateTransactionCode(),
                    Amount = createTransactionDTO.Amount,
                    PaymentType = createTransactionDTO.PaymentType,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                await _transactionRepository.AddAsync(transaction);
                await _transactionRepository.SaveChangesAsync();

                var transactionDTO = new TransactionDTO
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
                };

                return new ApiResponse<TransactionDTO>
                {
                    Data = transactionDTO,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<TransactionDTO>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred while creating the transaction: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<TransactionDTO>>> GetAllAsync()
        {
            try
            {
                var transactions = await _transactionRepository.GetAllAsync();

                var transactionDtos = transactions.Select(t => new TransactionDTO
                {
                    Id = t.Id,
                    SellerPackageId = t.SellerPackageId,
                    SellerPackageName = t.SellerPackage?.Name,
                    CompanyId = t.CompanyId,
                    CompanyName = t.Company?.Name,
                    TransactionCode = t.TransactionCode,
                    Amount = t.Amount,
                    PaymentType = t.PaymentType,
                    CreatedAt = t.CreatedAt,
                    IsDeleted = t.IsDeleted
                }).ToList();

                return new ApiResponse<IEnumerable<TransactionDTO>>
                {
                    Data = transactionDtos,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<TransactionDTO>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred while retrieving transactions: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<TransactionDTO>> GetByIdAsync(Guid id)
        {
            try
            {
                var transaction = await _transactionRepository.GetByIdAsync(id);

                if (transaction == null)
                {
                    return new ApiResponse<TransactionDTO>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Transaction not found"
                    };
                }

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

                return new ApiResponse<TransactionDTO>
                {
                    Data = transactionDTO,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<TransactionDTO>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred while retrieving the transaction: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<TransactionDTO>>> GetByCompanyIdAsync(Guid companyId)
        {
            try
            {
                var transactions = await _transactionRepository.GetByCompanyIdAsync(companyId);

                var transactionDtos = transactions.Select(t => new TransactionDTO
                {
                    Id = t.Id,
                    SellerPackageId = t.SellerPackageId,
                    SellerPackageName = t.SellerPackage?.Name,
                    CompanyId = t.CompanyId,
                    CompanyName = t.Company?.Name,
                    TransactionCode = t.TransactionCode,
                    Amount = t.Amount,
                    PaymentType = t.PaymentType,
                    CreatedAt = t.CreatedAt,
                    IsDeleted = t.IsDeleted
                }).ToList();

                return new ApiResponse<IEnumerable<TransactionDTO>>
                {
                    Data = transactionDtos,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<TransactionDTO>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred while retrieving transactions: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<TransactionDTO>>> GetBySellerPackageIdAsync(Guid sellerPackageId)
        {
            try
            {
                var transactions = await _transactionRepository.GetBySellerPackageIdAsync(sellerPackageId);

                var transactionDtos = transactions.Select(t => new TransactionDTO
                {
                    Id = t.Id,
                    SellerPackageId = t.SellerPackageId,
                    SellerPackageName = t.SellerPackage?.Name,
                    CompanyId = t.CompanyId,
                    CompanyName = t.Company?.Name,
                    TransactionCode = t.TransactionCode,
                    Amount = t.Amount,
                    PaymentType = t.PaymentType,
                    CreatedAt = t.CreatedAt,
                    IsDeleted = t.IsDeleted
                }).ToList();

                return new ApiResponse<IEnumerable<TransactionDTO>>
                {
                    Data = transactionDtos,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<TransactionDTO>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred while retrieving transactions: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<TransactionDTO>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var transactions = await _transactionRepository.GetByDateRangeAsync(startDate, endDate);

                var transactionDtos = transactions.Select(t => new TransactionDTO
                {
                    Id = t.Id,
                    SellerPackageId = t.SellerPackageId,
                    SellerPackageName = t.SellerPackage?.Name,
                    CompanyId = t.CompanyId,
                    CompanyName = t.Company?.Name,
                    TransactionCode = t.TransactionCode,
                    Amount = t.Amount,
                    PaymentType = t.PaymentType,
                    CreatedAt = t.CreatedAt,
                    IsDeleted = t.IsDeleted
                }).ToList();

                return new ApiResponse<IEnumerable<TransactionDTO>>
                {
                    Data = transactionDtos,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<TransactionDTO>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred while retrieving transactions: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<decimal>> GetTotalRevenueAsync(Guid companyId)
        {
            try
            {
                var totalRevenue = await _transactionRepository.GetTotalRevenueAsync(companyId);

                return new ApiResponse<decimal>
                {
                    Data = totalRevenue,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<decimal>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred while calculating revenue: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<string>> CancelTransactionAsync(Guid id)
        {
            try
            {
                var transaction = await _transactionRepository.GetByIdAsync(id);

                if (transaction == null)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Transaction not found"
                    };
                }

                // Business logic: Check if transaction can be cancelled (within 24 hours)
                if (DateTime.UtcNow.Subtract(transaction.CreatedAt).TotalHours > 24)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Transaction cannot be cancelled after 24 hours"
                    };
                }

                transaction.IsDeleted = true;
                transaction.UpdatedAt = DateTime.UtcNow;
                await _transactionRepository.SaveChangesAsync();

                return new ApiResponse<string>
                {
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = "Transaction cancelled successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred while cancelling the transaction: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<string>> CreatePayOSPaymentAsync(CreatePayOSPaymentRequestDto dto)
        {
            // Validate SellerPackage exists
            var sellerPackage = await _sellerPackageRepository.GetByIdAsync(dto.PackageId);
            if (sellerPackage == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = "SellerPackage not found"
                };
            }
            // Gọi PayOSService để tạo giao dịch
            var payUrl = await _payOSService.CreatePaymentAsync(dto);
            return new ApiResponse<string>
            {
                Data = payUrl,
                Success = true,
                MessageId = MessageId.I0000,
                Message = Message.GetMessageById(MessageId.I0000)
            };
        }

        private string GenerateTransactionCode()
        {
            return $"TXN{DateTime.UtcNow:yyyyMMddHHmmss}{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
} 