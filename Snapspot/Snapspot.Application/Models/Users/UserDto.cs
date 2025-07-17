using System;

namespace Snapspot.Application.Models.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public DateTime Dob { get; set; }
        public string PhoneNumber { get; set; }
        public string AvatarUrl { get; set; }
        public Guid RoleId { get; set; }
        public float Rating { get; set; }
        public bool IsApproved { get; set; }
        public string Bio { get; set; }
        
    }

    public class CreateUserDto
    {
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string Password { get; set; }
        public DateTime Dob { get; set; }
        public string PhoneNumber { get; set; }
        public string AvatarUrl { get; set; }
        public Guid RoleId { get; set; }
    }

    public class UpdateUserDto
    {
        public string Fullname { get; set; }
        public DateTime Dob { get; set; }
        public string PhoneNumber { get; set; }
        public string AvatarUrl { get; set; }
    }

    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
} 