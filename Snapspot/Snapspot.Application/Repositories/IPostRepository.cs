using Snapspot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.Repositories
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetPostsBySpotIdAsync(Guid spotId);
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<IEnumerable<Post>> SearchPostsAsync(string query);
        Task<bool> LikePostAsync(Guid postId, Guid userId);
        Task<bool> UnlikePostAsync(Guid postId, Guid userId);
        Task<Comment> CreateCommentAsync(Guid postId, Guid userId, string content);
        Task<List<Comment>> GetCommentsByPostIdAsync(Guid postId);
        Task<Post> CreatePostAsync(Post post);
    }
}
