using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Models.Provinces;
using Snapspot.Shared.Common;
using System.Collections.Generic;

namespace Snapspot.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerAnalyticsController : ControllerBase
    {


        [HttpGet("general-statistics")]
        public IActionResult GetGeneralStatistics()
        {
            var data = new GeneralStatisticsDto(
                    TotalUser: 1530,
                    TotalCompany: 78,
                    MonthyRevenue: 760000,
                    TotalBlog: 94
                );

            var response = new ApiResponse<GeneralStatisticsDto>
            {
                Data = data,
                Success = true,
            };

            return Ok(response);
        }

        [HttpGet("active-users")]
        public IActionResult GetActiveUsers()
        {
            var rng = new Random();
            var today = DateTime.Today;

            var data = Enumerable.Range(0, 7)
                .Select(i => new ActiveUserDto(
                    Name: today.AddDays(-i).Day.ToString(),
                    User: rng.Next(50, 201)
                ))
                .Reverse()
                .ToList();

            var response = new ApiResponse<List<ActiveUserDto>>
            {
                Data = data,
                Success = true,
                Message = "OK"
            };

            return Ok(response);
        }

        public record BlogStatisticDto(string Name, int Blog);

        [HttpGet("blogs")]
        public IActionResult GetBlogs()
        {
            var rng = new Random();
            var today = DateTime.Today;

            var data = Enumerable.Range(0, 7)
                .Select(i => new BlogStatisticDto(
                    Name: today.AddDays(-i).Day.ToString(),
                    Blog: rng.Next(0, 30) 
                ))
                .Reverse()
                .ToList();

            var response = new ApiResponse<List<BlogStatisticDto>>
            {
                Success = true,
                Message = "OK",
                Data = data
            };

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

    public record GeneralStatisticsDto(
        int TotalUser,
        int TotalCompany,
        decimal MonthyRevenue,
        int TotalBlog
    );

    public record ActiveUserDto(string Name, int User);
}
