using Microsoft.EntityFrameworkCore;
using Snapspot.Application.Models.Users;
using Snapspot.Domain.Entities;
using Snapspot.Infrastructure.Persistence.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            var user = await _context.Set<User>()
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

            if (user == null)
                return null;

            return MapToDto(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _context.Set<User>()
                .Include(u => u.Role)
                .Where(u => !u.IsDeleted)
                .ToListAsync();

            return users.Select(MapToDto);
        }

        public async Task<UserDto> CreateAsync(CreateUserDto createUserDto)
        {
            var existingUser = await _context.Set<User>()
                .FirstOrDefaultAsync(u => u.Email == createUserDto.Email && !u.IsDeleted);

            if (existingUser != null)
                throw new Exception("Email already exists");

            var user = new User
            {
                Email = createUserDto.Email,
                Fullname = createUserDto.Fullname,
                Password = HashPassword(createUserDto.Password),
                Dob = createUserDto.Dob,
                PhoneNumber = createUserDto.PhoneNumber,
                AvatarUrl = createUserDto.AvatarUrl,
                RoleId = createUserDto.RoleId,
                Rating = 0,
                IsApproved = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Set<User>().Add(user);
            await _context.SaveChangesAsync();

            return MapToDto(user);
        }

        public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto updateUserDto)
        {
            var user = await _context.Set<User>()
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

            if (user == null)
                throw new Exception("User not found");

            user.Fullname = updateUserDto.Fullname;
            user.Dob = updateUserDto.Dob;
            user.PhoneNumber = updateUserDto.PhoneNumber;
            user.AvatarUrl = updateUserDto.AvatarUrl;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToDto(user);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _context.Set<User>().FindAsync(id);
            if (user == null)
                return false;

            user.IsDeleted = true;
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UserDto> LoginAsync(UserLoginDto loginDto)
        {
            var user = await _context.Set<User>()
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email && !u.IsDeleted);

            if (user == null || user.Password != HashPassword(loginDto.Password))
                throw new Exception("Invalid email or password");

            return MapToDto(user);
        }

        public async Task<bool> ChangePasswordAsync(Guid id, string currentPassword, string newPassword)
        {
            var user = await _context.Set<User>().FindAsync(id);
            if (user == null)
                return false;

            if (user.Password != HashPassword(currentPassword))
                throw new Exception("Current password is incorrect");

            user.Password = HashPassword(newPassword);
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            var user = await _context.Set<User>()
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);

            return user != null ? MapToDto(user) : null;
        }

        public async Task<IEnumerable<UserDto>> GetThirdPartyUsersAsync()
        {
            var thirdPartyUsers = await _context.Set<User>()
                .Include(u => u.Role)
                .Where(u => !u.IsDeleted && u.Role.Name == "ThirdParty")
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            return thirdPartyUsers.Select(MapToDto);
        }

        public async Task<IEnumerable<UserDto>> GetRegularUsersAsync()
        {
            var regularUsers = await _context.Set<User>()
                .Include(u => u.Role)
                .Where(u => !u.IsDeleted && u.Role.Name == "User")
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            return regularUsers.Select(MapToDto);
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Fullname = user.Fullname,
                Dob = user.Dob,
                PhoneNumber = user.PhoneNumber,
                AvatarUrl = user.AvatarUrl,
                RoleId = user.RoleId,
                Rating = user.Rating,
                IsApproved = user.IsApproved
            };
        }
    }
} 