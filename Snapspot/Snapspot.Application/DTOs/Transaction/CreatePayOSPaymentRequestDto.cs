namespace Snapspot.Application.DTOs.Transaction
{
    public class CreatePayOSPaymentRequestDto
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
        public string BuyerEmail { get; set; }
        public string BuyerPhone { get; set; }
        public string BuyerName { get; set; }
        public Guid PackageId { get; set; }
        public Guid UserId { get; set; }
    }
} 