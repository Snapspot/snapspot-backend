using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.Models.Posts
{
    public class CreatePostRequestDto
    {
        public string Content { get; set; }
        public Guid? SpotId { get; set; }  // Optional - có thể đăng bài không gắn spot
        public List<string> ImageUrls { get; set; } = new List<string>();
    }
}
