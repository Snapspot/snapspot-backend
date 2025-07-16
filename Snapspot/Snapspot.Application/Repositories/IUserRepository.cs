using Snapspot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.Repositories
{
    public interface IUserRepository : IGenericRepository<User, Guid>
    {
        Task<bool> ExistsByEmailAsync(string email);
        Task<User?> LoginAsync(string email, string password);
        Task<User?> GetByUserIdAsync(Guid userId);
        Task<User?> GetByIdWithRoleAsync(Guid userId);
    }
}
