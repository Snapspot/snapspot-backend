using System;
using System.ComponentModel.DataAnnotations;

namespace Snapspot.Application.DTOs.Transaction
{
    public class CreateTransactionDTO
    {
        [Required]
        public Guid SellerPackageId { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        [Required]
        [StringLength(255)]
        public string TransactionCode { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(20)]
        public string PaymentType { get; set; }
    }
} 