using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.Services
{
    public interface IJwtService
    {
        string GenerateAccessToken(Guid userId, string role);
        Task<string> GenerateRefreshTokenAsync(Guid userId);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
        Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken);
    }
}
