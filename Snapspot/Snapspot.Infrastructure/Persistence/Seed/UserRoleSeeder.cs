using Microsoft.EntityFrameworkCore;
using Snapspot.Domain.Entities;
using Snapspot.Infrastructure.Persistence.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snapspot.Infrastructure.Persistence.Seed
{
    public static class UserRoleSeeder
    {
        public static void SeedData(AppDbContext context)
        {
            // Kiểm tra và thêm roles nếu chưa có
            if (!context.Roles.Any(r => r.Name == "User" || r.Name == "Admin" || r.Name == "ThirdParty"))
            {
                var roles = new List<Role>
                {
                    new Role { Id = Guid.NewGuid(), Name = "User", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Role { Id = Guid.NewGuid(), Name = "Admin", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Role { Id = Guid.NewGuid(), Name = "ThirdParty", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };

                context.Roles.AddRange(roles);
                context.SaveChanges();
            }

            // Lấy roles từ database
            var userRole = context.Roles.FirstOrDefault(r => r.Name == "User");
            var adminRole = context.Roles.FirstOrDefault(r => r.Name == "Admin");
            var thirdPartyRole = context.Roles.FirstOrDefault(r => r.Name == "ThirdParty");

            // Kiểm tra từng loại user mẫu và thêm nếu chưa có
            var users = new List<User>();

            // Thêm User role nếu chưa có user mẫu
            if (!context.Users.Any(u => u.Email == "user1@example.com"))
            {
                users.Add(new User
                {
                    Id = Guid.NewGuid(),
                    Email = "user1@example.com",
                    Fullname = "Normal User",
                    Password = BCrypt.Net.BCrypt.HashPassword("user1"),
                    Dob = new DateTime(1990, 1, 1),
                    PhoneNumber = "0123456789",
                    AvatarUrl = "https://example.com/avatar1.jpg",
                    RoleId = userRole.Id,
                    Rating = 0,
                    IsApproved = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            // Thêm 4 Admin roles nếu chưa có
            for (int i = 1; i <= 4; i++)
            {
                string email = $"admin{i}@example.com";
                if (!context.Users.Any(u => u.Email == email))
                {
                    users.Add(new User
                    {
                        Id = Guid.NewGuid(),
                        Email = email,
                        Fullname = $"Admin User {i}",
                        Password = BCrypt.Net.BCrypt.HashPassword("admin1"),
                        Dob = new DateTime(1990, 1, 1),
                        PhoneNumber = $"0123456{i:D3}",
                        AvatarUrl = $"https://example.com/admin{i}.jpg",
                        RoleId = adminRole.Id,
                        Rating = 0,
                        IsApproved = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
            }

            // Thêm 5 ThirdParty roles nếu chưa có
            for (int i = 1; i <= 5; i++)
            {
                string email = $"thirdparty{i}@example.com";
                if (!context.Users.Any(u => u.Email == email))
                {
                    users.Add(new User
                    {
                        Id = Guid.NewGuid(),
                        Email = email,
                        Fullname = $"Third Party User {i}",
                        Password = BCrypt.Net.BCrypt.HashPassword("thirdparty1"),
                        Dob = new DateTime(1990, 1, 1),
                        PhoneNumber = $"0987654{i:D3}",
                        AvatarUrl = $"https://example.com/thirdparty{i}.jpg",
                        RoleId = thirdPartyRole.Id,
                        Rating = 0,
                        IsApproved = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
            }

            // Thêm các user mới vào database nếu có
            if (users.Any())
            {
                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }
    }
} 