using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using Snapspot.Application.DTOs.Transaction;
using Snapspot.Application.Models.Requests.Payment;
using Snapspot.Application.Models.Responses.Payment;
using Snapspot.Application.UseCases.Interfaces.Transaction;
using Snapspot.Shared.Common;
using Snapspot.Shared.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Snapspot.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionUseCase _transactionUseCase;
        private readonly PayOS _payOS;
        private ILogger _logger;
        public TransactionController(ITransactionUseCase transactionUseCase, PayOS payOS, ILogger logger)
        {
            _transactionUseCase = transactionUseCase;
            _payOS = payOS;
            _logger = logger;
        }

        [HttpPost("payment-links")]
        [ProducesResponseType(typeof(ApiResponse<PaymentResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreatePaymentLink([FromBody] PaymentRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            ApiResponse<PaymentResponse> response;

            if (string.IsNullOrEmpty(userId))
            {
                response = new()
                {
                    Success = false,
                    Message = "User is not authenticated"
                };
            }
            else
            {
                response = await _transactionUseCase.CreatePaymentUrl(Guid.Parse(userId), request);
            }
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.ThirdParty)]
        public async Task<ActionResult<ApiResponse<TransactionDTO>>> CreateTransaction([FromBody] CreateTransactionDTO createTransactionDTO)
        {
            // Validate that the company ID matches the logged-in user's company
            var userCompanyId = User.FindFirst("CompanyId")?.Value;
            if (string.IsNullOrEmpty(userCompanyId) || Guid.Parse(userCompanyId) != createTransactionDTO.CompanyId)
            {
                return Forbid();
            }

            var result = await _transactionUseCase.CreateAsync(createTransactionDTO);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("webhook")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> HandleWebhookPayOS([FromBody] WebhookType request)
        {
            _logger.LogInformation("Api has been actived");

            var webHookData =  _transactionUseCase.VerifyPaymentWebhookData(request);
            _logger.LogInformation("Verify payment complete");
            if (webHookData ==  null)
            {
                _logger.LogInformation("can't verify paymentdata");

            }
            var response = await _transactionUseCase.CreateUserSubscription(webHookData.Data);
            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ActionResult<ApiResponse<System.Collections.Generic.IEnumerable<TransactionDTO>>>> GetAllTransactions()
        {
            var result = await _transactionUseCase.GetAllAsync();
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("company/{companyId}")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.ThirdParty}")]
        public async Task<ActionResult<ApiResponse<System.Collections.Generic.IEnumerable<TransactionDTO>>>> GetTransactionsByCompany(Guid companyId)
        {
            // If ThirdParty, validate that they can only see their own transactions
            if (User.IsInRole(RoleConstants.ThirdParty))
            {
                var userCompanyId = User.FindFirst("CompanyId")?.Value;
                if (string.IsNullOrEmpty(userCompanyId) || Guid.Parse(userCompanyId) != companyId)
                {
                    return Forbid();
                }
            }

            var result = await _transactionUseCase.GetByCompanyIdAsync(companyId);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.ThirdParty}")]
        public async Task<ActionResult<ApiResponse<TransactionDTO>>> GetTransactionById(Guid id)
        {
            var result = await _transactionUseCase.GetByIdAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);

            // If ThirdParty, validate that they can only see their own transactions
            if (User.IsInRole(RoleConstants.ThirdParty))
            {
                var userCompanyId = User.FindFirst("CompanyId")?.Value;
                if (string.IsNullOrEmpty(userCompanyId) || Guid.Parse(userCompanyId) != result.Data.CompanyId)
                {
                    return Forbid();
                }
            }

            return Ok(result);
        }

        [HttpGet("seller-package/{sellerPackageId}")]
        public async Task<ActionResult<ApiResponse<System.Collections.Generic.IEnumerable<TransactionDTO>>>> GetTransactionsBySellerPackage(Guid sellerPackageId)
        {
            var result = await _transactionUseCase.GetBySellerPackageIdAsync(sellerPackageId);
            return Ok(result);
        }

        [HttpGet("date-range")]
        public async Task<ActionResult<ApiResponse<System.Collections.Generic.IEnumerable<TransactionDTO>>>> GetTransactionsByDateRange(
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate)
        {
            var result = await _transactionUseCase.GetByDateRangeAsync(startDate, endDate);
            return Ok(result);
        }

        [HttpGet("revenue/{companyId}")]
        public async Task<ActionResult<ApiResponse<decimal>>> GetTotalRevenue(Guid companyId)
        {
            var result = await _transactionUseCase.GetTotalRevenueAsync(companyId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> CancelTransaction(Guid id)
        {
            var result = await _transactionUseCase.CancelTransactionAsync(id);
            return Ok(result);
        }

        [HttpPost("payos-payment")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<string>>> CreatePayOSPayment([FromBody] CreatePayOSPaymentRequestDto dto)
        {
            // Lấy UserId từ token nếu chưa truyền
            if (dto.UserId == Guid.Empty && User.Identity.IsAuthenticated)
            {
                var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userIdStr))
                    dto.UserId = Guid.Parse(userIdStr);
            }
            var result = await _transactionUseCase.CreatePayOSPaymentAsync(dto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }

        [HttpPost("payos-callback")]
        [AllowAnonymous]
        public IActionResult PayOSCallback([FromBody] PayOSCallbackDto callbackDto)
        {
            // TODO: Xử lý xác thực callback, cập nhật trạng thái giao dịch
            // Có thể gọi TransactionUseCase để cập nhật trạng thái
            return Ok();
        }
    }
} 