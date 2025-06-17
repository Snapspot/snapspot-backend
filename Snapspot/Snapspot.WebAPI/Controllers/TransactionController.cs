using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.DTOs.Transaction;
using Snapspot.Application.Interfaces;
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
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.ThirdParty)]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionDTO createTransactionDTO)
        {
            // Validate that the company ID matches the logged-in user's company
            var userCompanyId = User.FindFirst("CompanyId")?.Value;
            if (string.IsNullOrEmpty(userCompanyId) || Guid.Parse(userCompanyId) != createTransactionDTO.CompanyId)
            {
                return Forbid();
            }

            var result = await _transactionService.CreateTransactionAsync(createTransactionDTO);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpGet]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> GetAllTransactions()
        {
            var result = await _transactionService.GetAllTransactionsAsync();
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpGet("company/{companyId}")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.ThirdParty}")]
        public async Task<IActionResult> GetTransactionsByCompanyId(Guid companyId)
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

            var result = await _transactionService.GetTransactionsByCompanyIdAsync(companyId);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.ThirdParty}")]
        public async Task<IActionResult> GetTransactionById(Guid id)
        {
            var result = await _transactionService.GetTransactionByIdAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            // If ThirdParty, validate that they can only see their own transactions
            if (User.IsInRole(RoleConstants.ThirdParty))
            {
                var userCompanyId = User.FindFirst("CompanyId")?.Value;
                if (string.IsNullOrEmpty(userCompanyId) || Guid.Parse(userCompanyId) != result.Value.CompanyId)
                {
                    return Forbid();
                }
            }

            return Ok(result.Value);
        }
    }
} 