using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.UseCases.Interfaces.User;
using Snapspot.Shared.Common;

namespace Snapspot.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserUseCase _userUseCase;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userUseCase"></param>
        public UserController(IUserUseCase userUseCase)
        {
            _userUseCase = userUseCase;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAll([FromQuery] PagingRequest request)
        {
            var response = await _userUseCase.GetAllAsync(request);
            return Ok(response);
        }

        //[HttpGet("{userId}")]
        //public Task<IActionResult> GetByUserId([FromQuery] Guid userId)
        //{

        //}

        //[HttpPost]
        //public Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        //{

        //}

        //[HttpPut("{userId}")]
        //public Task<IActionResult> UpdateUserProfile([FromQuery] Guid userId, [FromBody] UpdateUserRequest request)
        //{

        //}

        //[HttpDelete("{userId}")]
        //public Task<IActionResult> DeleteByUserId([FromQuery] Guid userId)
        //{

        //}
    }
}
