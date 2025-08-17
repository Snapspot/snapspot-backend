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
            var data = new List<object>
            {
                new { id = 1, name = "Chi nhánh Quận 1", views = 845, rating = 4.5 },
                new { id = 2, name = "Chi nhánh Thủ Đức", views = 612, rating = 4.2 },
                new { id = 3, name = "Chi nhánh Cầu Giấy", views = 978, rating = 4.8 },
                new { id = 4, name = "Chi nhánh Hải Châu", views = 451, rating = 3.9 },
                new { id = 5, name = "Chi nhánh Đống Đa", views = 763, rating = 4.1 },
                new { id = 6, name = "Chi nhánh Bình Thạnh", views = 520, rating = 4.6 },
                new { id = 7, name = "Chi nhánh Nam Từ Liêm", views = 890, rating = 4.7 },
                new { id = 8, name = "Chi nhánh Tân Bình", views = 687, rating = 4.0 },
                new { id = 9, name = "Chi nhánh 11", views = 915, rating = 4.3 },
                new { id = 10, name = "Chi nhánh 7", views = 740, rating = 4.4 }
            };
            var response = new ApiResponse<object>()
            {
                Success = true,
                Message = "OK",
                Data = data
            };
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
