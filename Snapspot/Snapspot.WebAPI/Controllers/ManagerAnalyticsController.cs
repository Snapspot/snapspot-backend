using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Models.Provinces;
using Snapspot.Application.UseCases.Interfaces.Analytic;
using Snapspot.Application.UseCases.Interfaces.Province;
using Snapspot.Shared.Common;
using System.Collections.Generic;
using static Snapspot.Application.Repositories.ISellerPackageRepository;

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

        [HttpGet("monthy-statistics")]
        public async Task<IActionResult> GetMonthyStatistics()
        {
            var response = await _analyticUseCase.GetMonthyStatistics();

            return Ok(response);
        }

        [HttpGet("package-coverage")]
        public async Task<IActionResult> GetPackageCoverage()
        {
            var response = await _analyticUseCase.GetPackageCoverage();

            return Ok(response);
        }

       

        [HttpGet("package-revenue")]
        public async Task<IActionResult> GetPackageRevenue()
        {
            var response = await _analyticUseCase.GetPackageRevenue();

            return Ok(response);
        }
    }

    
}
