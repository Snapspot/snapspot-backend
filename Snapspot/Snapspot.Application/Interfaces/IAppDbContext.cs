using Microsoft.EntityFrameworkCore;
using Snapspot.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Snapspot.Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }
        DbSet<SellerPackage> SellerPackages { get; set; }
        DbSet<Company> Companies { get; set; }
        DbSet<Agency> Agencies { get; set; }
        DbSet<Feedback> Feedbacks { get; set; }
        DbSet<Spot> Spots { get; set; }
        DbSet<District> Districts { get; set; }
        DbSet<Province> Provinces { get; set; }
        DbSet<AgencyService> AgencyServices { get; set; }
        DbSet<Booking> Bookings { get; set; }
        DbSet<Transaction> Transactions { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
} 