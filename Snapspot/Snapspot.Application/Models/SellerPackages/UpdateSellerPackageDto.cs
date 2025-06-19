namespace Snapspot.Application.Models.SellerPackages
{
    public class UpdateSellerPackageDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxAgency { get; set; }
        public decimal Price { get; set; }
    }
} 