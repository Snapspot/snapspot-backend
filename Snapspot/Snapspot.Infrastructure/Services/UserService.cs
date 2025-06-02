using Microsoft.EntityFrameworkCore;
using Snapspot.Application.Models.Users;
using Snapspot.Application.Services;
using Snapspot.Domain.Entities;
using Snapspot.Infrastructure.Persistence.DBContext;
using Snapspot.Shared.Enums;
using System.Security.Cryptography;
using System.Text;

namespace Snapspot.Infrastructure.Services
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
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

            return user != null ? MapToDto(user) : null;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .Where(u => !u.IsDeleted)
                .ToListAsync();

            return users.Select(MapToDto);
        }

        public async Task<UserDto> CreateAsync(CreateUserDto createUserDto)
        {
            // Validate email uniqueness
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == createUserDto.Email && !u.IsDeleted);

            if (existingUser != null)
                throw new Exception("Email already exists");

            // Get default User role if roleId is not provided
            if (createUserDto.RoleId == Guid.Empty)
            {
                var defaultRole = await _context.Roles
                    .FirstOrDefaultAsync(r => r.Name == RoleEnum.User.ToString());
                
                if (defaultRole == null)
                    throw new Exception("Default User role not found in the system");
                
                createUserDto.RoleId = defaultRole.Id;
            }
            else
            {
                // Validate role exists
                var role = await _context.Roles
                    .FirstOrDefaultAsync(r => r.Id == createUserDto.RoleId && !r.IsDeleted);
                
                if (role == null)
                    throw new Exception($"Role with ID '{createUserDto.RoleId}' not found. Available roles are: Admin (1A73F130-B445-4F46-8F88-D4E4A4645E5C), ThirdParty (2B83F131-B445-4F46-8F88-D4E4A4645E5C), User (3C93F132-B445-4F46-8F88-D4E4A4645E5C)");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = createUserDto.Email,
                Fullname = createUserDto.Fullname,
                Password = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password),
                Dob = createUserDto.Dob,
                PhoneNumber = createUserDto.PhoneNumber ?? string.Empty,
                AvatarUrl = createUserDto.AvatarUrl ?? "https://th.bing.com/th/id/OIP.a9qb_VLfFjvlrDfc-iNLpgHaHa?rs=1&pid=ImgDetMain",
                RoleId = createUserDto.RoleId,
                Rating = 0,
                IsApproved = false,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return MapToDto(user);
        }

        public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto updateUserDto)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

            if (user == null)
                throw new Exception("User not found");

            user.Fullname = updateUserDto.Fullname;
            user.Dob = updateUserDto.Dob;
            user.PhoneNumber = updateUserDto.PhoneNumber ?? user.PhoneNumber;
            user.AvatarUrl = updateUserDto.AvatarUrl ?? user.AvatarUrl;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToDto(user);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            user.IsDeleted = true;
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UserDto> LoginAsync(UserLoginDto loginDto)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email && !u.IsDeleted);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
                throw new Exception("Invalid email or password");

            return MapToDto(user);
        }

        public async Task<bool> ChangePasswordAsync(Guid id, string currentPassword, string newPassword)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.Password))
                throw new Exception("Current password is incorrect");

            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);

            return user != null ? MapToDto(user) : null;
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