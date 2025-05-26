using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Snapspot.Application.Repositories;
using Snapspot.Application.Services;
using Snapspot.Domain.Entities;
using Snapspot.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IGenericRepository<RefreshToken, Guid> _repository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="jwtSettings"></param>
        /// <param name="repository"></param>
        public JwtService(IOptions<JwtSettings> jwtSettings, IGenericRepository<RefreshToken, Guid> repository)
        {
            _jwtSettings = jwtSettings.Value;
            _repository = repository;
        }

        /// <summary>
        /// GenerateAccessToken
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string GenerateAccessToken(Guid userId, string role)
        {
            var claims = new Claim[]
            {
               new(ClaimTypes.NameIdentifier, userId.ToString()),
               new(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: _jwtSettings.Issuer, audience: _jwtSettings.Audience, claims: claims, expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes), signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// GenerateRefreshTokenAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<string> GenerateRefreshTokenAsync(Guid userId)
        {
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var expiry = DateTime.UtcNow.AddDays(7);
            var tokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Token = refreshToken,
                ExpiryDate = expiry
            };

            _ = await _repository.AddAsync(tokenEntity);
            await _repository.SaveChangesAsync();

            return refreshToken;
        }

        /// <summary>
        /// GetPrincipalFromExpiredToken
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ValidateRefreshTokenAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var tokenEntity = (await _repository.FindAsync(x => x.Token == refreshToken, asNoTracking: true)).FirstOrDefault();

            return tokenEntity != null &&
                   tokenEntity.UserId == userId &&
                   tokenEntity.ExpiryDate > DateTime.UtcNow;
        }
    }
}
