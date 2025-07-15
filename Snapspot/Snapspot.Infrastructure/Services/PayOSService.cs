using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Snapspot.Application.DTOs.Transaction;
using Snapspot.Application.Services;
using Snapspot.Infrastructure.Options;

namespace Snapspot.Infrastructure.Services
{
    public class PayOSService : IPayOSService
    {
        private readonly HttpClient _httpClient;
        private readonly PayOSOptions _options;

        public PayOSService(HttpClient httpClient, IOptions<PayOSOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<string> CreatePaymentAsync(CreatePayOSPaymentRequestDto request)
        {
            // TODO: Build request body theo PayOS API
            var body = new
            {
                amount = request.Amount,
                description = request.Description,
                returnUrl = request.ReturnUrl,
                cancelUrl = request.CancelUrl,
                buyerEmail = request.BuyerEmail,
                buyerPhone = request.BuyerPhone,
                buyerName = request.BuyerName,
                orderCode = request.UserId.ToString() + "_" + request.PackageId.ToString()
            };
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("x-client-id", _options.ClientId);
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _options.ApiKey);
            var response = await _httpClient.PostAsync(_options.Endpoint + "/api/v1/payment-requests", content);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            // TODO: Parse response để lấy link thanh toán
            using var doc = JsonDocument.Parse(responseString);
            var payUrl = doc.RootElement.GetProperty("data").GetProperty("checkoutUrl").GetString();
            return payUrl;
        }

        public bool ValidateCallback(PayOSCallbackDto callbackDto)
        {
            // TODO: Validate checksum/signature nếu cần
            return true;
        }
    }
} 