using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Models.Agencies;
using Snapspot.Application.UseCases.Interfaces.Feedback;
using Snapspot.Shared.Common;
using Snapspot.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Snapspot.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackUseCase _feedbackUseCase;

        public FeedbackController(IFeedbackUseCase feedbackUseCase)
        {
            _feedbackUseCase = feedbackUseCase;
        }

        /// <summary>
        /// Lấy tất cả feedback với phân trang (Admin only)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ActionResult<ApiResponse<PagingResponse<FeedbackDto>>>> GetAll(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
            try
            {
                // Validate parameters
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                var result = await _feedbackUseCase.GetPagedAsync(pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<PagingResponse<FeedbackDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = ex.Message
                });
            }
        }

        /// <summary>
        /// Lấy feedback theo ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<FeedbackDto>>> GetById(Guid id)
        {
            try
            {
                var result = await _feedbackUseCase.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<FeedbackDto>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = ex.Message
                });
            }
        }

        /// <summary>
        /// Lấy tất cả feedback của một đại lý với phân trang
        /// </summary>
        [HttpGet("agency/{agencyId}")]
        public async Task<ActionResult<ApiResponse<PagingResponse<FeedbackDto>>>> GetByAgencyId(
            Guid agencyId,
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
            try
            {
                // Validate parameters
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                var result = await _feedbackUseCase.GetPagedByAgencyIdAsync(agencyId, pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<PagingResponse<FeedbackDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = ex.Message
                });
            }
        }

        /// <summary>
        /// Lấy tất cả feedback của một user với phân trang
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponse<PagingResponse<FeedbackDto>>>> GetByUserId(
            Guid userId,
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
            try
            {
                // Kiểm tra quyền: chỉ user đó hoặc admin mới được xem
                var currentUserId = GetCurrentUserId();
                if (currentUserId != userId && !User.IsInRole(RoleConstants.Admin))
                    return Forbid();

                // Validate parameters
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                var result = await _feedbackUseCase.GetPagedByUserIdAsync(userId, pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<PagingResponse<FeedbackDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = ex.Message
                });
            }
        }

        /// <summary>
        /// Tạo feedback mới
        /// </summary>
        [HttpPost]
        [Authorize(Roles = RoleConstants.User)]
        public async Task<ActionResult<ApiResponse<FeedbackDto>>> Create([FromBody] CreateFeedbackDto createFeedbackDto)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var result = await _feedbackUseCase.CreateAsync(createFeedbackDto, currentUserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<FeedbackDto>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = ex.Message
                });
            }
        }

        /// <summary>
        /// Cập nhật feedback
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = RoleConstants.User)]
        public async Task<ActionResult<ApiResponse<FeedbackDto>>> Update(Guid id, [FromBody] UpdateFeedbackDto updateFeedbackDto)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var result = await _feedbackUseCase.UpdateAsync(id, updateFeedbackDto, currentUserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<FeedbackDto>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = ex.Message
                });
            }
        }

        /// <summary>
        /// Xóa feedback
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleConstants.User)]
        public async Task<ActionResult<ApiResponse<string>>> Delete(Guid id)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var result = await _feedbackUseCase.DeleteAsync(id, currentUserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = ex.Message
                });
            }
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                throw new UnauthorizedAccessException("Invalid user ID");
            return userId;
        }
    }
} 