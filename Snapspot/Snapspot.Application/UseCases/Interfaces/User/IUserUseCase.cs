using Snapspot.Application.Models.Requests.User;
using Snapspot.Application.Models.Responses.User;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.UseCases.Interfaces.User
{
    public interface IUserUseCase
    {
        Task<ApiResponse<string>> CreateUserAsync(CreateUserRequest request);
        Task<ApiResponse<string>> UpdateProfileAsync(Guid userId, UpdateUserRequest request);
        Task<ApiResponse<string>> ChangePasswordAsync(Guid userId, ChangePasswordRequest request);
        Task<ApiResponse<string>> DeleteAccountAsync(Guid userId);
        Task<ApiResponse<PagingResponse<GetUserResponse>>> GetAllAsync(PagingRequest request);
        Task<ApiResponse<GetUserResponse>> GetByIdAsync(Guid userId);
    }
}
