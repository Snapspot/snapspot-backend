using Snapspot.Shared.Common;
using System.Threading.Tasks;

namespace Snapspot.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<string>> LoginAsync(string email, string password);
        Task<Result<string>> RefreshTokenAsync(string token);
    }
} 