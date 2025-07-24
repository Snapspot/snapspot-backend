using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.Models.Posts
{
    public class CommentResponseDto
    {
        public Guid CommentId { get; set; }
        public UserInfoDto User { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
