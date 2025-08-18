using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Snapspot.Application.Repositories.ISellerPackageRepository;
using static Snapspot.Application.UseCases.Implementations.Analytic.AnalyticUseCase;

namespace Snapspot.Application.UseCases.Interfaces.Analytic
{
    public interface IAnalyticUseCase
    {
        Task<ApiResponse<List<AnalyticActiveUserDto>>> GetActiveUsers();
        Task<ApiResponse<AnalyticGeneralStatisticsDto>> GetGeneralStatistics();
        Task<ApiResponse<List<AnalyticBlogStatisticDto>>> GetStatisticNewBlogs();
        Task<ApiResponse<AnalyticMonthyStatisticsDto>> GetMonthyStatistics();
        Task<ApiResponse<List<PackageCoverageDto>>> GetPackageCoverage();
        Task<ApiResponse<object>> GetPackageRevenue();
        Task<ApiResponse<List<AnalyticCompanyViewDto>>> GetViewsData(string? userId);
        Task<ApiResponse<List<AnalyticAgenciesDataDto>>> GetAgenciesData(string? userId);

    }
}
