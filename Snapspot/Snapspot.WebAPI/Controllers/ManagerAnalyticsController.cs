using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Models.Provinces;
using Snapspot.Application.UseCases.Interfaces.Analytic;
using Snapspot.Application.UseCases.Interfaces.Province;
using Snapspot.Shared.Common;
using System.Collections.Generic;

namespace Snapspot.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerAnalyticsController : ControllerBase
    {
        private readonly IAnalyticUseCase _analyticUseCase;


        public ManagerAnalyticsController(IAnalyticUseCase analyticUseCase)
        {
            _analyticUseCase = analyticUseCase;
        }

        [HttpGet("general-statistics")]
        public async Task<IActionResult> GetGeneralStatistics()
        {
            var response = await _analyticUseCase.GetGeneralStatistics();

            return Ok(response);
        }

        [HttpGet("active-users")]
        public async Task<IActionResult> GetActiveUsers()
        {
            var response = await _analyticUseCase.GetActiveUsers();

            return Ok(response);
        }
        

        [HttpGet("blogs")]
        public async Task<IActionResult> GetBlogs()
        {
            var response = await _analyticUseCase.GetStatisticNewBlogs();

            return Ok(response);
        }
        public record MonthyStatisticsDto(int NewUser, int NewCompany, int NewBlog);

        [HttpGet("monthy-statistics")]
        public IActionResult GetMonthyStatistics()
        {
            var data = new MonthyStatisticsDto(
                NewUser: 14,
                NewCompany: 3,
                NewBlog: 34
            );

            var response = new ApiResponse<MonthyStatisticsDto>
            {
                Success = true,
                Message = "OK",
                Data = data
            };

            return Ok(response);
        }

        public record PackageCoverageDto(string Name, int Value);

        [HttpGet("package-coverage")]
        public IActionResult GetPackageCoverage()
        {
            var data = new List<PackageCoverageDto>
            {
                new("Gói Cơ bản", 23),
                new("Gói cao cấp", 7),
                new("Gói tiêu chuẩn", 9)
            };

            var response = new ApiResponse<List<PackageCoverageDto>>
            {
                Success = true,
                Message = "OK",
                Data = data
            };

            return Ok(response);
        }

        public record PackageRevenueDto(string Name, decimal Uv, decimal Pv, decimal Atm);

        [HttpGet("package-revenue")]
        public IActionResult GetPackageRevenue()
        {
            var data = new List<PackageRevenueDto>
            {
                new("Doanh thu", 144000, 98000, 80000)
            };

            var response = new ApiResponse<List<PackageRevenueDto>>
            {
                Success = true,
                Message = "OK",
                Data = data
            };

            return Ok(response);
        }
    }

    
}
