using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.UseCases.Interfaces.Post;

namespace Snapspot.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostUseCase _postUseCase;

        public PostsController(IPostUseCase postUseCase)
        {
            _postUseCase = postUseCase;
        }

        [HttpGet("{spotId}")]
        public async Task<IActionResult> GetBySpotId(Guid spotId)
        {
            var result = await _postUseCase.GetPostsBySpotIdAsync(spotId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var result = await _postUseCase.GetAllPostsAsync();
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            var result = await _postUseCase.SearchPostsAsync(q);
            return Ok(result);
        }
        [Authorize]
        [HttpPost("{postId}/like")]
        public async Task<IActionResult> LikePost(Guid postId)
        {
            // Lấy userId từ JWT
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id" || c.Type.EndsWith("nameidentifier"));
            if (userIdClaim == null)
                return Unauthorized();

            Guid userId = Guid.Parse(userIdClaim.Value);

            var result = await _postUseCase.LikePostAsync(postId, userId);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
