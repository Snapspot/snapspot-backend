using Microsoft.EntityFrameworkCore;
using Snapspot.Application.Models.Requests.User;
using Snapspot.Application.Models.Responses.User;
using Snapspot.Application.Repositories;
using Snapspot.Application.UseCases.Interfaces.User;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.UseCases.Implementations.User
{
    public class UserUseCase : IUserUseCase
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userRepository"></param>
        public UserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<ApiResponse<string>> ChangePasswordAsync(Guid userId, ChangePasswordRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<string>> CreateUserAsync(CreateUserRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<string>> DeleteAccountAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<PagingResponse<GetUserResponse>>> GetAllAsync(PagingRequest request)
        {
            var pagingResponse = await _userRepository.FindPagedAsync(
                predicate: query => !query.IsDeleted,
                include: query => query.Include(x => x.Role),
                pageNumber: request.PageIndex,
                pageSize: request.PageSize,
                asNoTracking: true,
                orderBy: query => query.OrderByDescending(x => x.CreatedAt)
                                       .ThenByDescending(x => x.UpdatedAt)
                                       .ThenByDescending(x => x.Email)
            );

            if (pagingResponse.TotalItems == 0)
            {
                return ApiResponse<PagingResponse<GetUserResponse>>.Fail(MessageId.E0005);
            }

            var data = new PagingResponse<GetUserResponse>
            {
                Items = pagingResponse.Items.Select(x => new GetUserResponse
                {
                    Dob = x.Dob,
                    Email = x.Email,
                    RoleId = x.RoleId,
                    RoleName = x.Role.Name,
                    UserId = x.Id,
                    FullName = x.Fullname,
                }),
                PageIndex = pagingResponse.PageIndex,
                PageSize = pagingResponse.PageSize,
                TotalItems = pagingResponse.TotalItems,
            };

            return ApiResponse<PagingResponse<GetUserResponse>>.Ok(data); ;
        }

        public async Task<ApiResponse<GetUserResponse>> GetByIdAsync(Guid userId)
        {
            // Lấy user từ repository, bao gồm cả Role
           
            var user = await _userRepository.GetByIdWithRoleAsync(userId);
            // Kiểm tra user có tồn tại và chưa bị xóa không
            if (user == null || user.IsDeleted)
            {
                return ApiResponse<GetUserResponse>.Fail(MessageId.E0005);
            }

            // Map sang response
            var response = new GetUserResponse
            {
                UserId = user.Id,
                FullName = user.Fullname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Dob = user.Dob,
                RoleId = user.RoleId,
                RoleName = user.Role?.Name // Có thể null nếu không include Role
            };

            return ApiResponse<GetUserResponse>.Ok(response);
        }

        public Task<ApiResponse<string>> UpdateProfileAsync(Guid userId, UpdateUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
