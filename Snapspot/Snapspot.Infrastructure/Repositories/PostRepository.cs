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

        public async Task<Comment> CreateCommentAsync(Guid postId, Guid userId, string content)
        {
            var post = await _context.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == postId);

            var user = await _context.Users.FindAsync(userId);

            if (post == null || user == null)
                return null;

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                PostId = postId,
                UserId = userId,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            
            return await _context.Comments
                    .Include(c => c.User)
                    .Include(c => c.Post)
                    .ThenInclude(p => p.Spot)
                    .FirstOrDefaultAsync(c => c.Id == comment.Id);
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

        public async Task<List<Comment>> GetCommentsByPostIdAsync(Guid postId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Post)
                .ThenInclude(p => p.Spot)
                .Where(c => c.PostId == postId)
                .OrderByDescending(c => c.CreatedAt)
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

        public async Task<bool> LikePostAsync(Guid postId, Guid userId)
        {
            var post = await _context.Posts
        .Include(p => p.LikePosts)
        .FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null) return false;

            // Kiểm tra đã like chưa
            if (post.LikePosts.Any(lp => lp.UserId == userId))
                return false;

            post.LikePosts.Add(new LikePost
            {
                PostId = postId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return true;
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

        public async Task<bool> UnlikePostAsync(Guid postId, Guid userId)
        {
            var post = await _context.Posts
                .Include(p => p.LikePosts)
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null) return false;

            var like = post.LikePosts.FirstOrDefault(lp => lp.UserId == userId);
            if (like == null)
                return false;

            post.LikePosts.Remove(like);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
