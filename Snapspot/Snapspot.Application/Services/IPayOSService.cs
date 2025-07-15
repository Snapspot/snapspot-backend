using System.Threading.Tasks;
using Snapspot.Application.DTOs.Transaction;

namespace Snapspot.Application.Services
{
    public interface IPayOSService
    {
        Task<string> CreatePaymentAsync(CreatePayOSPaymentRequestDto request);
        bool ValidateCallback(PayOSCallbackDto callbackDto);
    }
} 