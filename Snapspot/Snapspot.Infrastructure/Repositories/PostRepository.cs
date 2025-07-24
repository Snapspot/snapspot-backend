using Microsoft.EntityFrameworkCore;
using Snapspot.Application.Repositories;
using Snapspot.Domain.Entities;
using Snapspot.Infrastructure.Persistence.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Spot)
                .Include(p => p.Images)
                .Include(p => p.LikePosts)
                .Include(p => p.Comments)
                .Include(p => p.SavePosts)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsBySpotIdAsync(Guid spotId)
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Spot)
                .Include(p => p.Images)
                .Include(p => p.LikePosts)
                .Include(p => p.Comments)
                .Where(p => p.SpotId == spotId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> SearchPostsAsync(string query)
        {
            return await _context.Posts
         .Include(p => p.User)
         .Include(p => p.Spot)
         .Include(p => p.Images)
         .Include(p => p.LikePosts)
         .Include(p => p.Comments)
         .Include(p => p.SavePosts)
         .Where(p => p.Content.Contains(query))
         .ToListAsync();
        }
    }
}
