using Snapspot.Domain.Base;
using System;

namespace Snapspot.Domain.Entities
{
    public class Image : BaseEntity<Guid>
    {
        public string Uri { get; set; }
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }
    }
} 