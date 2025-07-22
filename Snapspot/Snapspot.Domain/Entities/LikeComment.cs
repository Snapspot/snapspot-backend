using System;

namespace Snapspot.Domain.Entities
{
    public class LikeComment
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid CommentId { get; set; }
        public virtual Comment Comment { get; set; }
    }
} 