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

        public async Task<Post> CreatePostAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<bool> DeletePostAsync(Guid postId, Guid userId)
        {
            var post = await _context.Posts
                 .Include(p => p.Images)
                 .Include(p => p.Comments)
                 .Include(p => p.LikePosts)
                 .Include(p => p.SavePosts)
                 .FirstOrDefaultAsync(p => p.Id == postId && p.UserId == userId);

            if (post == null)
                return false;

            // Xóa các related entities trước
            _context.Images.RemoveRange(post.Images);
            _context.Comments.RemoveRange(post.Comments);
            _context.LikePosts.RemoveRange(post.LikePosts);
            _context.SavePosts.RemoveRange(post.SavePosts);

            // Xóa post
            _context.Posts.Remove(post);

            await _context.SaveChangesAsync();
            return true;
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

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Spot)
                .Include(p => p.Images)
                .Include(p => p.Comments)
                .Include(p => p.LikePosts)
                .Include(p => p.SavePosts)
                .FirstOrDefaultAsync(p => p.Id == postId);
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
        public async Task<bool> SavePostAsync(Guid postId, Guid userId)
        {
            var existingSave = await _context.SavePosts
                .FirstOrDefaultAsync(sp => sp.PostId == postId && sp.UserId == userId);

            if (existingSave != null)
                return false;

            var savePost = new SavePost
            {
                PostId = postId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _context.SavePosts.AddAsync(savePost);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnsavePostAsync(Guid postId, Guid userId)
        {
            var savePost = await _context.SavePosts
                .FirstOrDefaultAsync(sp => sp.PostId == postId && sp.UserId == userId);

            if (savePost == null)
                return false;

            _context.SavePosts.Remove(savePost);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Post>> GetSavedPostsByUserIdAsync(Guid userId)
        {
            return await _context.SavePosts
                .Where(sp => sp.UserId == userId)
                .Include(sp => sp.Post)
                    .ThenInclude(p => p.User)
                .Include(sp => sp.Post)
                    .ThenInclude(p => p.Spot)
                .Include(sp => sp.Post)
                    .ThenInclude(p => p.Images)
                .Include(sp => sp.Post)
                    .ThenInclude(p => p.LikePosts)
                .Include(sp => sp.Post)
                    .ThenInclude(p => p.Comments)
                .Include(sp => sp.Post)
                    .ThenInclude(p => p.SavePosts)
                .OrderByDescending(sp => sp.CreatedAt)
                .Select(sp => sp.Post)
                .ToListAsync();
        }

        public async Task<bool> IsPostSavedByUserAsync(Guid postId, Guid userId)
        {
            return await _context.SavePosts
                .AnyAsync(sp => sp.PostId == postId && sp.UserId == userId);
        }

        public async Task<int> GetTotalBlogAsync()
        {
            return await _context.Posts.CountAsync();
        }

        public async Task<int> CountNewBlogByDate(DateTime date)
        {
            var specificDate = date.Date;

            var count = await _context.Posts
                .CountAsync(ua => ua.CreatedAt.Date == specificDate);

            return count;
        }

        public async Task<int> CountNewBlogInMonthAsync()
        {
            var firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            var totalAmount = await _context.Posts
                .Where(t => t.CreatedAt >= firstDayOfMonth)
                .CountAsync();

            return totalAmount;
        }

        public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(Guid userId)
        {
            return await _context.Posts
                .Where(p => p.UserId == userId)
                .Include(p => p.User)
                .Include(p => p.Spot)
                .Include(p => p.Images)
                .Include(p => p.LikePosts)
                .Include(p => p.Comments)
                .Include(p => p.SavePosts)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}
