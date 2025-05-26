using Microsoft.EntityFrameworkCore;
using Snapspot.Application.Repositories;
using Snapspot.Domain.Entities;
using Snapspot.Infrastructure.Persistence.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="context"></param>
    public class UserRepository(AppDbContext context) : GenericRepository<User, Guid>(context), IUserRepository
    {
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email) != null;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _context.Users.Include(x => x.Role).AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
            var isSamePassword = BCrypt.Net.BCrypt.Verify(password, user?.Password);
            if (user == null || !isSamePassword) return null;
            return user;
        }
    }
}
