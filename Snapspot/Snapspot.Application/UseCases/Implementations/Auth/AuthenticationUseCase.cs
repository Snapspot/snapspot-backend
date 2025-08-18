using FluentValidation;
using Snapspot.Application.Models.Requests.Auth;
using Snapspot.Application.Models.Responses.Auth;
using Snapspot.Application.Repositories;
using Snapspot.Application.Services;
using Snapspot.Application.UseCases.Interfaces.Auth;
using Snapspot.Application.Validators.Auth;
using Snapspot.Domain.Entities;
using Snapspot.Shared.Common;
using Snapspot.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.UseCases.Implementations.Auth
{
    public class AuthenticationUseCase : IAuthenticationUseCase
    {
        private readonly RegisterRequestValidator _registerRequestValidator;
        private readonly LoginRequestValidator _loginRequestValidator;
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<Role, Guid> _roleRepository;
        private readonly IJwtService _jwtService;
        private readonly IActiveUserRepository _activeUserRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="registerRequestValidator"></param>
        /// <param name="loginRequestValidator"></param>
        /// <param name="userRepository"></param>
        /// <param name="roleRepository"></param>
        /// <param name="jwtService"></param>
        public AuthenticationUseCase(RegisterRequestValidator registerRequestValidator, LoginRequestValidator loginRequestValidator, IUserRepository userRepository, IGenericRepository<Role, Guid> roleRepository, IJwtService jwtService, IActiveUserRepository activeUserRepository)
        {
            _registerRequestValidator = registerRequestValidator;
            _loginRequestValidator = loginRequestValidator;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _jwtService = jwtService;
            _activeUserRepository = activeUserRepository;
        }

        public async Task<ApiResponse<TokenResponse>> LoginAsync(LoginRequest request)
        {
            await _loginRequestValidator.ValidateAndThrowAsync(request);
            var user = await _userRepository.LoginAsync(request.Email, request.Password);
            if (user == null)
            {
                return new ApiResponse<TokenResponse>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = Message.GetMessageById(MessageId.E0000)
                };
            }

            var tokenResponse = new TokenResponse
            {
                AccessToken = _jwtService.GenerateAccessToken(user.Id, user.Role.Name),
                RefreshToken = await _jwtService.GenerateRefreshTokenAsync(user.Id),
            };

            await _activeUserRepository.CheckLogin(user.Id); //Mark user as login today

            return new ApiResponse<TokenResponse>
            {
                Data = tokenResponse,
                Success = true,
                MessageId = MessageId.I0000,
                Message = Message.GetMessageById(MessageId.I0000)
            };
        }

        public async Task<ApiResponse<string>> RegisterAsync(RegisterRequest request)
        {
            await _registerRequestValidator.ValidateAndThrowAsync(request);

            // Role from request; default to User if missing
            var requestedRole = string.IsNullOrWhiteSpace(request.Role) ? "User" : request.Role;

            if (!Enum.TryParse<RoleEnum>(requestedRole, true, out var roleEnum))
            {
                roleEnum = RoleEnum.User;
            }

            var role = (await _roleRepository.FindAsync(
                x => x.Name == roleEnum.ToString(),
                asNoTracking: true)).FirstOrDefault();

            if (role == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = "Requested role doesn't exist."
                };
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new Domain.Entities.User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Fullname = request.Email,
                Password = hashedPassword,
                PhoneNumber = request.PhoneNumber,
                Dob = request.Dob,
                AvatarUrl = "https://th.bing.com/th/id/OIP.a9qb_VLfFjvlrDfc-iNLpgHaHa?rs=1&pid=ImgDetMain",
                RoleId = role.Id,
                Bio = "",
                Rating = 0.0f,
                IsApproved = false
            };

            _ = await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                MessageId = MessageId.I0000,
                Message = Message.GetMessageById(MessageId.I0000)
            };

        }

        public async Task<ApiResponse<TokenResponse>> GetNewAccessToken(TokenRequest request)
        {
            var isValid = await _jwtService.ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (isValid)
            {
                var user = await _userRepository.GetByUserIdAsync(request.UserId);
                if (user == null)
                {
                    return new ApiResponse<TokenResponse>
                    {
                        Success = false,
                        Message = Message.GetMessageById(MessageId.E0000),
                        MessageId = MessageId.E0000,
                    };
                }

                var responseToken = new TokenResponse
                {
                    AccessToken = _jwtService.GenerateAccessToken(user.Id, user.Role.Name),
                    RefreshToken = request.RefreshToken,
                };

                return new ApiResponse<TokenResponse>
                {
                    Data = responseToken,
                    Success = true,
                    Message = Message.GetMessageById(MessageId.I0000),
                    MessageId = MessageId.I0000,
                };
            }

            return new ApiResponse<TokenResponse>
            {
                Success = false,
                Message = Message.GetMessageById(MessageId.E0000),
                MessageId = MessageId.E0000,
            };
        }
    }
}
