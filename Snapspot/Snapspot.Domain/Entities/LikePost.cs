using System;

namespace Snapspot.Domain.Entities
{
    public class LikePost
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 