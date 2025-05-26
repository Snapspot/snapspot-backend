using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Models.Requests.Auth;
using Snapspot.Application.UseCases.Interfaces.Auth;
using Snapspot.Application.Validators.Auth;
using Snapspot.Shared.Common;

namespace Snapspot.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly RegisterRequestValidator _registerRequestValidator;
        private readonly IAuthenticationUseCase _authUseCase;

        public AuthenticationController(IAuthenticationUseCase authUseCase)
        {
            _authUseCase = authUseCase;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var response = await _authUseCase.RegisterAsync(request);

            if (response.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authUseCase.LoginAsync(request);

            if (response.Success) return Ok(response);
            return Unauthorized(response);
        }
    }
}
