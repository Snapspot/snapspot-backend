using Snapspot.Domain.Base;
using System;
using System.Collections.Generic;

namespace Snapspot.Domain.Entities
{
    public class Comment : BaseEntity<Guid>
    {
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }
        public virtual ICollection<LikeComment> LikeComments { get; set; }
        public Comment()
        {
            LikeComments = new HashSet<LikeComment>();
        }
    }
} 