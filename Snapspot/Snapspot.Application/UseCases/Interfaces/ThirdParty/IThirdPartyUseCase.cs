using Snapspot.Application.Models.Requests.Auth;
using Snapspot.Application.Models.Responses.Auth;
using Snapspot.Application.Models.Responses.ThirdParty;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.UseCases.Interfaces.ThirdParty
{
    public interface IThirdPartyUseCase
    {
        Task<ApiResponse<List<GetThirdPartyAgencyResponse>>> GetAgencies(string? userId);
        Task<ApiResponse<GetSerllerPackageInfor>> GetSellerPackageInfo(string? userId);
        Task<ApiResponse<List<GetUserFeedback>>> GetUserFeedback(string? userId);
    }
}
