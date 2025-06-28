using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.DTOs.Transaction;
using Snapspot.Application.UseCases.Interfaces.Transaction;
using Snapspot.Shared.Common;
using Snapspot.Shared.Constants;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Snapspot.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionUseCase _transactionUseCase;

        public TransactionController(ITransactionUseCase transactionUseCase)
        {
            _transactionUseCase = transactionUseCase;
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
    }
} 