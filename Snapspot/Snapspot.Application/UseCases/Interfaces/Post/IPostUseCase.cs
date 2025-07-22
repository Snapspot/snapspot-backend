using Snapspot.Application.Models.Posts;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.UseCases.Interfaces.Post
{
    public interface IPostUseCase
    {
        Task<ApiResponse<List<PostResponseDto>>> GetPostsBySpotIdAsync(Guid spotId);
        Task<ApiResponse<List<PostResponseDto>>> GetAllPostsAsync();
    }
}
