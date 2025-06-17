using Microsoft.EntityFrameworkCore;
using Snapspot.Application.Interfaces;
using Snapspot.Domain.Entities;
using Snapspot.Infrastructure.Persistence.DBContext;
using Snapspot.Shared.Common;
using System.Threading.Tasks;

namespace Snapspot.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<string>> LoginAsync(string email, string password)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);

            if (user == null)
                return Result<string>.Failure("User not found");

            if (!VerifyPassword(password, user.Password))
                return Result<string>.Failure("Invalid password");

            // Generate and return JWT token
            return Result<string>.Success("JWT_TOKEN");
        }

        public async Task<Result<string>> RefreshTokenAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token);

            if (refreshToken == null)
                return Result<string>.Failure("Invalid refresh token");

            if (refreshToken.ExpiryDate < System.DateTime.UtcNow)
                return Result<string>.Failure("Refresh token expired");

            // Generate and return new JWT token
            return Result<string>.Success("NEW_JWT_TOKEN");
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            // Implement password verification logic
            return true;
        }
    }
} 