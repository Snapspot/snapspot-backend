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
            var data = new
            {
                avarta = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTfqRWbHHwmAgqXU0XUu9jaByZiu6JNHabfhA&s",
                rating = 5,
                agencyCount = 4,
                companyName = "FPT Viet Nam",
                email = "fpt@gmail.com.vn",
                phoneNumber = "090909091203",
                address = "12321 Nguyen abc, bac, Thủ Đức, Viet Nam",
                website = "https://fpt.com.vn"
            };
            var response = new ApiResponse<object>()
            {
                Success = true,
                Message = "OK",
                Data = data
            };
            return Ok(response);
        }
    }
}
