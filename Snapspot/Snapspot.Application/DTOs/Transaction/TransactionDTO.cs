using System;

namespace Snapspot.Application.DTOs.Transaction
{
    public class TransactionDTO
    {
        public Guid Id { get; set; }
        public Guid SellerPackageId { get; set; }
        public string SellerPackageName { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string TransactionCode { get; set; }
        public decimal Amount { get; set; }
        public string PaymentType { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
} 