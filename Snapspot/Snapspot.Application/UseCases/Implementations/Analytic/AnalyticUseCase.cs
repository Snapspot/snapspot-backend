using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Repositories;
using Snapspot.Application.UseCases.Interfaces.Analytic;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Snapspot.Application.UseCases.Implementations.Analytic
{
    public class AnalyticUseCase : IAnalyticUseCase
    {
        private readonly IActiveUserRepository _activeUserRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IPostRepository _postRepository;
        private readonly ITransactionRepository _transactionRepository;

        public AnalyticUseCase(IActiveUserRepository activeUserRepository, IUserRepository userRepository, ICompanyRepository companyRepository, IPostRepository postRepository, ITransactionRepository transactionRepository)
        {
            _activeUserRepository = activeUserRepository;
            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _postRepository = postRepository;
            _transactionRepository = transactionRepository;
        }

        public record AnalyticBlogStatisticDto(string Name, int Blog);

        public async Task<ApiResponse<List<AnalyticBlogStatisticDto>>> GetStatisticNewBlogs()
        {
            var today = DateTime.Today;
            var data = new List<AnalyticBlogStatisticDto>();
            for (int i = 0; i < 7; i++)
            {
                DateTime specificDate = today.AddDays(-i);
                int blogCount = await _postRepository.CountNewBlogByDate(specificDate);
                data.Add(new AnalyticBlogStatisticDto(Name: specificDate.Day.ToString(), Blog: blogCount));
            }

            var response = new ApiResponse<List<AnalyticBlogStatisticDto>>
            {
                Data = data,
                Success = true,
                Message = "OK"
            };

            return response;
        }


        public record AnalyticGeneralStatisticsDto(
            int TotalUser,
            int TotalCompany,
            decimal MonthyRevenue,
            int TotalBlog
        );
        public async Task<ApiResponse<AnalyticGeneralStatisticsDto>> GetGeneralStatistics()
        {
            var data = new AnalyticGeneralStatisticsDto(
                    TotalUser: await _userRepository.GetTotalUserAsync(),
                    TotalCompany: await _companyRepository.GetTotalCompanyAsync(),
                    MonthyRevenue: await _transactionRepository.GetTotalAmountInCurrentMonth(),
                    TotalBlog: await _postRepository.GetTotalBlogAsync()
                );


            var response = new ApiResponse<AnalyticGeneralStatisticsDto>
            {
                Data = data,
                Success = true,
            };

            return response;
        }

        public record AnalyticActiveUserDto(string Name, int User);
        public async Task<ApiResponse<List<AnalyticActiveUserDto>>> GetActiveUsers()
        {
            var today = DateTime.Today;
            var data = new List<AnalyticActiveUserDto>();
            for (int i = 0; i < 7; i++)
            {
                DateTime specificDate = today.AddDays(-i);
                // Chờ tác vụ hoàn thành trước khi bắt đầu tác vụ tiếp theo
                int userCount = await _activeUserRepository.CountActiveUserByDate(specificDate);
                data.Add(new AnalyticActiveUserDto(Name: specificDate.Day.ToString(), User: userCount));
            }

            var response = new ApiResponse<List<AnalyticActiveUserDto>>
            {
                Data = data,
                Success = true,
                Message = "OK"
            };

            return response;
        }


    }
}
