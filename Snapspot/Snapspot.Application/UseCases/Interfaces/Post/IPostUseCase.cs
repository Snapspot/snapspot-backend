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
        Task<IEnumerable<PostResponseDto>> SearchPostsAsync(string query);
        Task<ApiResponse<bool>> LikePostAsync(Guid postId, Guid userId);
        Task<ApiResponse<bool>> UnlikePostAsync(Guid postId, Guid userId);
        Task<ApiResponse<CommentResponseDto>> CreateCommentAsync(Guid postId, Guid userId, CreateCommentRequestDto request);
        Task<ApiResponse<List<CommentResponseDto>>> GetCommentsByPostIdAsync(Guid postId);
        Task<ApiResponse<PostResponseDto>> CreatePostAsync(Guid userId, CreatePostRequestDto request);
        Task<ApiResponse<bool>> DeletePostAsync(Guid postId, Guid userId);
        Task<ApiResponse<bool>> SavePostAsync(Guid postId, Guid userId);
        Task<ApiResponse<bool>> UnsavePostAsync(Guid postId, Guid userId);
        Task<ApiResponse<List<SavedPostDto>>> GetSavedPostsAsync(Guid userId);
        Task<ApiResponse<bool>> IsPostSavedAsync(Guid postId, Guid userId);
        Task<ApiResponse<List<PostResponseDto>>> GetMyPostsAsync(Guid userId);  
    }
}
