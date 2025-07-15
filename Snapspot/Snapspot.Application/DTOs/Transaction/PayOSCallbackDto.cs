namespace Snapspot.Application.DTOs.Transaction
{
    public class PayOSCallbackDto
    {
        public string OrderCode { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string Signature { get; set; }
        public string Description { get; set; }
    }
} 