using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Models.Requests.User;
using Snapspot.Application.Models.Responses.User;
using Snapspot.Application.UseCases.Interfaces.User;
using Snapspot.Shared.Common;
using System;
using System.Threading.Tasks;

namespace Snapspot.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly IUserUseCase _userUseCase;

        public UsersController(IUserUseCase userUseCase)
        {
            _userUseCase = userUseCase;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] PagingRequest request)
        {
            var result = await _userUseCase.GetAllAsync(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User,ThirdParty")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _userUseCase.GetByIdAsync(id);
            if (result == null || !result.Success)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest createUserRequest)
        {
            try
            {
                var result = await _userUseCase.CreateUserAsync(createUserRequest);
                if (!result.Success)
                    return BadRequest(result.Message);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User,ThirdParty")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest updateUserRequest)
        {
            try
            {
                var result = await _userUseCase.UpdateProfileAsync(id, updateUserRequest);
                if (!result.Success)
                    return BadRequest(result.Message);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userUseCase.DeleteAccountAsync(id);
            if (!result.Success)
                return NotFound(result.Message);
            return NoContent();
        }

        [HttpPost("{id}/change-password")]
        public async Task<IActionResult> ChangePassword(Guid id, [FromBody] Snapspot.Application.Models.Requests.User.ChangePasswordRequest request)
        {
            try
            {
                var result = await _userUseCase.ChangePasswordAsync(id, request);
                if (!result.Success)
                    return NotFound(result.Message);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("profile")]
        [Authorize] 
        public async Task<IActionResult> GetProfile()
        {
            
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Unauthorized();
            }

            var result = await _userUseCase.GetByIdAsync(userId);
            if (result == null || !result.Success)
                return NotFound();

            return Ok(result);
        }
       
    }
} 