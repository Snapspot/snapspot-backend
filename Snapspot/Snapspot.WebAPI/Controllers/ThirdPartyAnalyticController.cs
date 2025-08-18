using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.UseCases.Interfaces.Analytic;
using Snapspot.Shared.Common;
using System.Security.Claims;

namespace Snapspot.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThirdPartyAnalyticController : ControllerBase
    {
        private readonly IAnalyticUseCase _analyticUseCase;
        public ThirdPartyAnalyticController(IAnalyticUseCase analyticUseCase)
        {
            _analyticUseCase = analyticUseCase;
        }

        [HttpGet("views")]
        public async Task<IActionResult> GetViewsData()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = await _analyticUseCase.GetViewsData(userId);
            return Ok(response);
        }

        [HttpGet("agencies")]
        public async Task<IActionResult> GetAgenciesData()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = await _analyticUseCase.GetAgenciesData(userId);
            return Ok(response);
        }

        [HttpGet("company-info")]
        public async Task<IActionResult> GetCompanyInfo()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = await _analyticUseCase.GetCompanyInfo(userId);
            return Ok(response);
        }
    }
}
