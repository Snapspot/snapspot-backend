using Snapspot.Application.Models.Posts;
using Snapspot.Application.Repositories;
using Snapspot.Application.UseCases.Interfaces.Post;
using Snapspot.Domain.Entities;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.UseCases.Implementations.Post
{
    public class PostUseCase : IPostUseCase
    {
        private readonly IPostRepository _postRepository;
        public PostUseCase(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        private double ComputePostScore(
            int likes,
            int comments,
            int saves,
            int ageInDays,
            int likesLast24h,
            int commentsLast24h,
            int savesLast24h)
        {
            const double w1 = 3.0;
            const double w2 = 4.0;
            const double w3 = 5.0;
            const double k = 7.0;

            double likeScore = Math.Log(likes + 1) * w1;
            double commentScore = Math.Log(comments + 1) * w2;
            double saveScore = Math.Log(saves + 1) * w3;
            double freshness = Math.Exp(-ageInDays / k) * 100;
            double hotScore = (likesLast24h + commentsLast24h + savesLast24h) > 10 ? 20 : 0;

            return likeScore + commentScore + saveScore + freshness + hotScore;
        }

        public async Task<ApiResponse<List<PostResponseDto>>> GetPostsBySpotIdAsync(Guid spotId)
        {
            try
            {
                var posts = await _postRepository.GetPostsBySpotIdAsync(spotId);
                if (posts == null || !posts.Any())
                {
                    return new ApiResponse<List<PostResponseDto>>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "No posts found"
                    };
                }

                var now = DateTime.UtcNow;
                var postDtos = posts
                    .Select(post => new
                    {
                        Post = post,
                        Score = ComputePostScore(
                            post.LikePosts?.Count ?? 0,
                            post.Comments?.Count ?? 0,
                            post.SavePosts?.Count ?? 0,
                            (now - post.CreatedAt).Days,
                            post.LikePosts?.Count(lp => (now - lp.CreatedAt).TotalHours <= 24) ?? 0,
                            post.Comments?.Count(c => (now - c.CreatedAt).TotalHours <= 24) ?? 0,
                            post.SavePosts?.Count(sp => (now - sp.CreatedAt).TotalHours <= 24) ?? 0
                        )
                    })
                    .OrderByDescending(x => x.Score)
                    .Select(x => new PostResponseDto
                    {
                        PostId = x.Post.Id.ToString(),
                        User = new UserInfoDto
                        {
                            Name = x.Post.User?.Fullname,
                            SpotId = x.Post.SpotId,
                            Spotname = x.Post.Spot?.Name,
                            Avatar = x.Post.User?.AvatarUrl
                        },
                        Content = x.Post.Content,
                        ImageUrl = x.Post.Images?.Select(img => img.Uri).ToList() ?? new List<string>(),
                        Likes = x.Post.LikePosts?.Count ?? 0,
                        Comments = x.Post.Comments?.Count ?? 0,
                        Timestamp = x.Post.CreatedAt
                    })
                    .ToList();

                return new ApiResponse<List<PostResponseDto>>
                {
                    Data = postDtos,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<PostResponseDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0001,
                    Message = ex.Message
                };
            }
        }

        public async Task<ApiResponse<List<PostResponseDto>>> GetAllPostsAsync()
        {
            try
            {
                var posts = await _postRepository.GetAllPostsAsync();
                if (posts == null || !posts.Any())
                {
                    return new ApiResponse<List<PostResponseDto>>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "No posts found"
                    };
                }

                var now = DateTime.UtcNow;
                var postDtos = posts
                    .Select(post => new
                    {
                        Post = post,
                        Score = ComputePostScore(
                            post.LikePosts?.Count ?? 0,
                            post.Comments?.Count ?? 0,
                            post.SavePosts?.Count ?? 0,
                            (now - post.CreatedAt).Days,
                            post.LikePosts?.Count(lp => (now - lp.CreatedAt).TotalHours <= 24) ?? 0,
                            post.Comments?.Count(c => (now - c.CreatedAt).TotalHours <= 24) ?? 0,
                            post.SavePosts?.Count(sp => (now - sp.CreatedAt).TotalHours <= 24) ?? 0
                        )
                    })
                    .OrderByDescending(x => x.Score)
                    .Select(x => new PostResponseDto
                    {
                        PostId = x.Post.Id.ToString(),
                        User = new UserInfoDto
                        {
                            Name = x.Post.User?.Fullname,
                            SpotId = x.Post.SpotId,
                            Spotname = x.Post.Spot?.Name,
                            Avatar = x.Post.User?.AvatarUrl
                        },
                        Content = x.Post.Content,
                        ImageUrl = x.Post.Images?.Select(img => img.Uri).ToList() ?? new List<string>(),
                        Likes = x.Post.LikePosts?.Count ?? 0,
                        Comments = x.Post.Comments?.Count ?? 0,
                        Timestamp = x.Post.CreatedAt
                    })
                    .ToList();

                return new ApiResponse<List<PostResponseDto>>
                {
                    Data = postDtos,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<PostResponseDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0001,
                    Message = ex.Message
                };
            }
        }

        public async Task<IEnumerable<PostResponseDto>> SearchPostsAsync(string query)
        {
            var posts = await _postRepository.SearchPostsAsync(query);
            var now = DateTime.UtcNow;
            var postDtos = posts.Select(post => new PostResponseDto
            {
                PostId = post.Id.ToString(),
                User = new UserInfoDto
                {
                    Name = post.User?.Fullname,
                    SpotId = post.SpotId,
                    Spotname = post.Spot?.Name,
                    Avatar = post.User?.AvatarUrl
                },
                Content = post.Content,
                ImageUrl = post.Images?.Select(img => img.Uri).ToList() ?? new List<string>(),
                Likes = post.LikePosts?.Count ?? 0,
                Comments = post.Comments?.Count ?? 0,
                Timestamp = post.CreatedAt
            }).ToList();

            return postDtos;
        }

        public async Task<ApiResponse<bool>> LikePostAsync(Guid postId, Guid userId)
        {
            try
            {
                var result = await _postRepository.LikePostAsync(postId, userId);
                if (!result)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "You have already liked this post.",
                        Data = false
                    };
                }
                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Post liked successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = false
                };
            }
        }

        public async Task<ApiResponse<bool>> UnlikePostAsync(Guid postId, Guid userId)
        {
            try
            {
                var result = await _postRepository.UnlikePostAsync(postId, userId);
                if (!result)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "You have not liked this post yet.",
                        Data = false
                    };
                }
                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Post unliked successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = false
                };
            }
        }

        public async Task<ApiResponse<CommentResponseDto>> CreateCommentAsync(Guid postId, Guid userId, CreateCommentRequestDto request)
        {
            try
            {
                var comment = await _postRepository.CreateCommentAsync(postId, userId, request.Content);
                if (comment == null)
                {
                    return new ApiResponse<CommentResponseDto>
                    {
                        Success = false,
                        Message = "Post not found or cannot comment.",
                        Data = null
                    };
                }

                var response = new CommentResponseDto
                {
                    CommentId = comment.Id,
                    User = new UserInfoDto
                    {
                        UserId = comment.User.Id,
                        Name = comment.User.Fullname,
                        SpotId = comment.Post?.SpotId, 
                        Spotname = comment.Post?.Spot?.Name, 
                        Avatar = comment.User.AvatarUrl
                    },
                    Content = comment.Content,
                    Timestamp = comment.CreatedAt
                };

                return new ApiResponse<CommentResponseDto>
                {
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = "Comment created successfully.",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<CommentResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<List<CommentResponseDto>>> GetCommentsByPostIdAsync(Guid postId)
        {
            var comments = await _postRepository.GetCommentsByPostIdAsync(postId);

            if (comments == null || !comments.Any())
            {
                return new ApiResponse<List<CommentResponseDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000, 
                    Message = "No comments found for this post.",
                    Data = null
                };
            }

            var commentDtos = comments.Select(comment => new CommentResponseDto
            {
                CommentId = comment.Id,
                User = new UserInfoDto
                {
                    UserId = comment.User.Id,
                    Name = comment.User.Fullname,
                    Avatar = comment.User.AvatarUrl,
                    SpotId = comment.Post?.SpotId,
                    Spotname = comment.Post?.Spot?.Name
                },
                Content = comment.Content,
                Timestamp = comment.CreatedAt
            }).ToList();

            return new ApiResponse<List<CommentResponseDto>>
            {
                Success = true,
                MessageId = MessageId.I0000,
                Message = "Get comments successfully.",
                Data = commentDtos
            };
        }

        public async Task<ApiResponse<PostResponseDto>> CreatePostAsync(Guid userId, CreatePostRequestDto request)
        {
            try
            {
                // Validate request
                if (string.IsNullOrWhiteSpace(request.Content))
                {
                    return new ApiResponse<PostResponseDto>
                    {
                        Success = false,
                        Message = "Content cannot be empty",
                        Data = null
                    };
                }
                // Create new post entity
                var post = new Snapspot.Domain.Entities.Post
                {
                    Content = request.Content,
                    UserId = userId,
                    SpotId = request.SpotId,
                    CreatedAt = DateTime.UtcNow
                };

                // Add images if provided
                if (request.ImageUrls != null && request.ImageUrls.Any())
                {
                    foreach (var imageUrl in request.ImageUrls)
                    {
                        post.Images.Add(new Image { Uri = imageUrl });
                    }
                }

                // Save to database
                var createdPost = await _postRepository.CreatePostAsync(post);

                // Return response
                var response = new PostResponseDto
                {
                    PostId = createdPost.Id.ToString(),
                    User = new UserInfoDto
                    {
                        Name = createdPost.User?.Fullname,
                        SpotId = createdPost.SpotId,
                        Spotname = createdPost.Spot?.Name,
                        Avatar = createdPost.User?.AvatarUrl
                    },
                    Content = createdPost.Content,
                    ImageUrl = createdPost.Images?.Select(img => img.Uri).ToList() ?? new List<string>(),
                    Likes = 0,
                    Comments = 0,
                    Timestamp = createdPost.CreatedAt
                };

                return new ApiResponse<PostResponseDto>
                {
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = "Post created successfully",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<PostResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<bool>> DeletePostAsync(Guid postId, Guid userId)
        {
            try
            {
                // Kiểm tra post có tồn tại không
                var existingPost = await _postRepository.GetPostByIdAsync(postId);
                if (existingPost == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Post not found",
                        Data = false
                    };
                }

                // Kiểm tra user có quyền xóa post không (chỉ owner mới được xóa)
                if (existingPost.UserId != userId)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "You don't have permission to delete this post",
                        Data = false
                    };
                }

                // Thực hiện xóa post
                var result = await _postRepository.DeletePostAsync(postId, userId);
                if (!result)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Failed to delete post",
                        Data = false
                    };
                }

                return new ApiResponse<bool>
                {
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = "Post deleted successfully",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = false
                };
            }
        }
    }
}
