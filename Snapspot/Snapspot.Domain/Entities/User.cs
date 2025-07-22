using Snapspot.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Domain.Entities
{
    public class User : BaseEntity<Guid>
    {
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string Password { get; set; }
        public DateTime Dob { get; set; }
        public string PhoneNumber { get; set; }
        public string AvatarUrl { get; set; }
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }
        public string Bio { get; set; }
        public float Rating { get; set; }
        public bool IsApproved { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<LikePost> LikePosts { get; set; }
        public virtual ICollection<LikeComment> LikeComments { get; set; }
        public virtual ICollection<SavePost> SavePosts { get; set; }

        public User()
        {
            Feedbacks = new HashSet<Feedback>();
            Posts = new HashSet<Post>();
            Comments = new HashSet<Comment>();
            LikePosts = new HashSet<LikePost>();
            LikeComments = new HashSet<LikeComment>();
            SavePosts = new HashSet<SavePost>();
        }
    }
}
