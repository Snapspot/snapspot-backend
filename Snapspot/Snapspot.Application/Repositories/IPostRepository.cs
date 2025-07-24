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
    }
}
