using Snapspot.Domain.Base;
using System;

namespace Snapspot.Domain.Entities
{
    public class Comment : BaseEntity<Guid>
    {
        public string Content { get; set; }
        public int Rating { get; set; }
        public bool IsApproved { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid AgencyId { get; set; }
        public virtual Agency Agency { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 