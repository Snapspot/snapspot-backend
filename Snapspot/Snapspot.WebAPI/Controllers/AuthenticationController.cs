using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Models.Requests.Auth;
using Snapspot.Application.Validators.Auth;
using Snapspot.Shared.Common;

namespace Snapspot.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly RegisterRequestValidator _registerRequestValidator;

        public AuthenticationController(RegisterRequestValidator registerRequestValidator)
        {
            _registerRequestValidator = registerRequestValidator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            await _registerRequestValidator.ValidateAndThrowAsync(request);

            return Ok(new ApiResponse<string>
            {
                Success = true,
                MessageId = MessageId.I0000,
                Message = Message.GetMessageById(MessageId.I0000),
            });
        }
    }
}
