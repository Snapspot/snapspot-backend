using Snapspot.Domain.Base;
using System;
using System.Collections.Generic;

namespace Snapspot.Domain.Entities
{
    public class Post : BaseEntity<Guid>
    {
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid? SpotId { get; set; }
        public virtual Spot Spot { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<LikePost> LikePosts { get; set; }
        public virtual ICollection<SavePost> SavePosts { get; set; }
        public Post()
        {
            Images = new HashSet<Image>();
            Comments = new HashSet<Comment>();
            LikePosts = new HashSet<LikePost>();
            SavePosts = new HashSet<SavePost>();
        }
    }
} 