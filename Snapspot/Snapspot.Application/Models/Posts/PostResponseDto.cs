using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.Models.Posts
{
    public class PostResponseDto
    {
        public string PostId { get; set; }
        public UserInfoDto User { get; set; }
        public string Content { get; set; }
        public List<string> ImageUrl { get; set; }
        public int Likes { get; set; }
        public int Comments { get; set; }
        public DateTime Timestamp { get; set; }
        public string Query { get; set; }
    }

    public class UserInfoDto
    {
        public string Name { get; set; }
        public Guid? SpotId { get; set; }
        public string Spotname { get; set; }
        public string Avatar { get; set; }
    }
}
