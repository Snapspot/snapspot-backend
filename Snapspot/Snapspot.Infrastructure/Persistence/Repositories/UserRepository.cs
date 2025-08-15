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
        public async Task<int> CountNewUserInMonthAsync()
        {
            var firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            var totalAmount = await _context.Users
                .Where(t => t.CreatedAt >= firstDayOfMonth)
                .CountAsync();

            return totalAmount;
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email) != null;
        }

        public async Task<User?> GetByIdWithRoleAsync(Guid userId)
        {
            return await _context.Users
         .Include(u => u.Role)
         .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Users.AsNoTracking().Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == userId && !x.IsDeleted);
        }

        public async Task<int> GetTotalUserAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _context.Users.Include(x => x.Role).AsNoTracking().FirstOrDefaultAsync(x => x.Email == email && !x.IsDeleted);
            var isSamePassword = BCrypt.Net.BCrypt.Verify(password, user?.Password);
            if (user == null || !isSamePassword) return null;
            return user;
        }
    }
}
