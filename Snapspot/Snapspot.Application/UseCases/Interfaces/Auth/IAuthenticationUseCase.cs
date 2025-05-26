using Snapspot.Application.Models.Requests.Auth;
using Snapspot.Application.Models.Responses.Auth;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.UseCases.Interfaces.Auth
{
    public interface IAuthenticationUseCase
    {
        Task<ApiResponse<string>> RegisterAsync(RegisterRequest request);
        Task<ApiResponse<TokenResponse>> LoginAsync(LoginRequest request);
        Task<ApiResponse<TokenResponse>> GetNewAccessToken(TokenRequest request);
    }
}
