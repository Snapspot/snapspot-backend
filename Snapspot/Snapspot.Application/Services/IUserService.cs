using Snapspot.Application.Models.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Application.Services
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(Guid id);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> CreateAsync(CreateUserDto createUserDto);
        Task<UserDto> UpdateAsync(Guid id, UpdateUserDto updateUserDto);
        Task<bool> DeleteAsync(Guid id);
        Task<UserDto> LoginAsync(UserLoginDto loginDto);
        Task<bool> ChangePasswordAsync(Guid id, string currentPassword, string newPassword);
        Task<UserDto> GetByEmailAsync(string email);
        Task<IEnumerable<UserDto>> GetThirdPartyUsersAsync();
        Task<IEnumerable<UserDto>> GetRegularUsersAsync();
    }
} 