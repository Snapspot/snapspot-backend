using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.Models.Requests.Payment
{
    public class PaymentRequest
    {
        [Required]
        public Guid SellerPackageId { get; set; }
        [Required]
        public int Month { get; set; }
    }
}
