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
    }
}
