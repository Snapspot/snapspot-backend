using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.Models.Responses.Payment
{
    public class PaymentResponse
    {
        public string CheckoutUrl { get; set; } = string.Empty;
    }
}
