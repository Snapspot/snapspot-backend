using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Models.Agencies;
using Snapspot.Application.Services;
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
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        /// <summary>
        /// Lấy tất cả feedback với phân trang (Admin only)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ActionResult<PagingResponse<FeedbackDto>>> GetAll(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
            try
            {
                // Validate parameters
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                var feedbacks = await _feedbackService.GetPagedAsync(pageNumber, pageSize);
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy feedback theo ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackDto>> GetById(Guid id)
        {
            try
            {
                var feedback = await _feedbackService.GetByIdAsync(id);
                if (feedback == null)
                    return NotFound();

                return Ok(feedback);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy tất cả feedback của một đại lý với phân trang
        /// </summary>
        [HttpGet("agency/{agencyId}")]
        public async Task<ActionResult<PagingResponse<FeedbackDto>>> GetByAgencyId(
            Guid agencyId,
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
            try
            {
                // Validate parameters
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                var feedbacks = await _feedbackService.GetByAgencyIdPagedAsync(agencyId, pageNumber, pageSize);
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy tất cả feedback của một user với phân trang
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<PagingResponse<FeedbackDto>>> GetByUserId(
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

                var feedbacks = await _feedbackService.GetByUserIdPagedAsync(userId, pageNumber, pageSize);
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy tất cả feedback đã được phê duyệt với phân trang
        /// </summary>
        [HttpGet("approved")]
        public async Task<ActionResult<PagingResponse<FeedbackDto>>> GetApprovedFeedbacks(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
            try
            {
                // Validate parameters
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                var feedbacks = await _feedbackService.GetApprovedFeedbacksPagedAsync(pageNumber, pageSize);
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy tất cả feedback chờ phê duyệt với phân trang (Admin only)
        /// </summary>
        [HttpGet("pending")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ActionResult<PagingResponse<FeedbackDto>>> GetPendingFeedbacks(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
            try
            {
                // Validate parameters
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                var feedbacks = await _feedbackService.GetPendingFeedbacksPagedAsync(pageNumber, pageSize);
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Tạo feedback mới
        /// </summary>
        [HttpPost]
        [Authorize(Roles = RoleConstants.User)]
        public async Task<ActionResult<FeedbackDto>> Create([FromBody] CreateFeedbackDto createFeedbackDto)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var feedback = await _feedbackService.CreateAsync(createFeedbackDto, currentUserId);
                return CreatedAtAction(nameof(GetById), new { id = feedback.Id }, feedback);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật feedback
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = RoleConstants.User)]
        public async Task<ActionResult<FeedbackDto>> Update(Guid id, [FromBody] UpdateFeedbackDto updateFeedbackDto)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var feedback = await _feedbackService.UpdateAsync(id, updateFeedbackDto, currentUserId);
                return Ok(feedback);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Xóa feedback
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleConstants.User)]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var result = await _feedbackService.DeleteAsync(id, currentUserId);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Phê duyệt/từ chối feedback (Admin only)
        /// </summary>
        [HttpPut("{id}/approve")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ActionResult<FeedbackDto>> Approve(Guid id, [FromBody] ApproveFeedbackDto approveFeedbackDto)
        {
            try
            {
                var feedback = await _feedbackService.ApproveAsync(id, approveFeedbackDto);
                return Ok(feedback);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException("Không thể xác định user hiện tại");

            return userId;
        }
    }
} 