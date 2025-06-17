using Microsoft.EntityFrameworkCore;
using Snapspot.Application.Interfaces;
using Snapspot.Domain.Base;
using Snapspot.Domain.Entities;
using Snapspot.Shared.Enums;

namespace Snapspot.Infrastructure.Persistence.DBContext
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<SellerPackage> SellerPackages { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Spot> Spots { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<AgencyService> AgencyServices { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Name)
                      .IsRequired()
                      .HasMaxLength(50)
                      .HasColumnName("Name");

                entity.Property(r => r.IsDeleted)
                      .HasDefaultValue(false);

                entity.Property(r => r.CreatedAt)
                      .HasColumnType("datetime");

                entity.Property(r => r.UpdatedAt)
                      .HasColumnType("datetime");
            });

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasIndex(u => u.Email)
                      .IsUnique();

                entity.Property(u => u.Fullname)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(u => u.Password)
                      .IsRequired()
                      .HasMaxLength(60);

                entity.Property(u => u.Dob)
                      .HasColumnType("date");

                entity.Property(u => u.PhoneNumber)
                      .HasMaxLength(20);

                entity.Property(u => u.AvatarUrl)
                      .HasColumnType("nvarchar(max)");

                entity.Property(u => u.IsDeleted)
                      .HasDefaultValue(false);

                entity.Property(u => u.CreatedAt)
                      .HasColumnType("datetime");

                entity.Property(u => u.UpdatedAt)
                      .HasColumnType("datetime");

                entity.HasOne(u => u.Role)
                      .WithMany(r => r.Users)
                      .HasForeignKey(u => u.RoleId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(u => u.Company)
                      .WithOne(c => c.User)
                      .HasForeignKey<Company>(c => c.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.Feedbacks)
                      .WithOne(c => c.User)
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SellerPackage>().ToTable("SellerPackage");
            modelBuilder.Entity<SellerPackage>(entity =>
            {
                entity.HasKey(sp => sp.Id);

                entity.Property(sp => sp.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(sp => sp.Description)
                      .IsRequired()
                      .HasMaxLength(500);

                entity.Property(sp => sp.Price)
                      .HasColumnType("decimal(18,2)");

                entity.Property(sp => sp.MaxAgency)
                      .IsRequired();

                entity.Property(sp => sp.SellingCount)
                      .HasDefaultValue(0);

                entity.Property(sp => sp.IsDeleted)
                      .HasDefaultValue(false);

                entity.Property(sp => sp.CreatedAt)
                      .HasColumnType("datetime");

                entity.Property(sp => sp.UpdatedAt)
                      .HasColumnType("datetime");

                entity.HasMany(sp => sp.Companies)
                      .WithMany(c => c.SellerPackages)
                      .UsingEntity(j => j.ToTable("CompanySellerPackage"));
            });

            modelBuilder.Entity<RefreshToken>().ToTable("RefreshToken");
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(rt => rt.Id);

                entity.Property(rt => rt.Token)
                      .IsRequired();

                entity.Property(rt => rt.ExpiryDate)
                      .HasColumnType("datetime");

                entity.Property(rt => rt.CreatedAt)
                      .HasColumnType("datetime");

                entity.Property(rt => rt.UpdatedAt)
                      .HasColumnType("datetime");

                entity.Property(rt => rt.IsDeleted)
                      .HasDefaultValue(false);

                entity.HasOne(rt => rt.User)
                      .WithMany()
                      .HasForeignKey(rt => rt.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Company>().ToTable("Company");
            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.HasOne(c => c.User)
                      .WithOne()
                      .HasForeignKey<Company>(c => c.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(c => c.Agencies)
                      .WithOne(a => a.Company)
                      .HasForeignKey(a => a.CompanyId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Agency>().ToTable("Agency");
            modelBuilder.Entity<Agency>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.HasOne(a => a.Company)
                      .WithMany(c => c.Agencies)
                      .HasForeignKey(a => a.CompanyId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Spot)
                      .WithMany()
                      .HasForeignKey(a => a.SpotId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(a => a.Feedbacks)
                      .WithOne(c => c.Agency)
                      .HasForeignKey(c => c.AgencyId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(a => a.Services)
                      .WithMany(s => s.Agencies)
                      .UsingEntity(j => j.ToTable("AgencyServiceMapping"));
            });

            modelBuilder.Entity<Feedback>().ToTable("Feedback");
            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.HasOne(c => c.User)
                      .WithMany()
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Agency)
                      .WithMany(a => a.Feedbacks)
                      .HasForeignKey(c => c.AgencyId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Province>().ToTable("Province");
            modelBuilder.Entity<Province>(entity =>
            {
                entity.HasKey(p => p.Id);
            });

            modelBuilder.Entity<District>().ToTable("District");
            modelBuilder.Entity<District>(entity =>
            {
                entity.HasKey(d => d.Id);

                entity.HasOne(d => d.Province)
                      .WithMany(p => p.Districts)
                      .HasForeignKey(d => d.ProvinceId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Spot>().ToTable("Spot");
            modelBuilder.Entity<Spot>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(s => s.Description)
                      .IsRequired()
                      .HasMaxLength(500);

                entity.Property(s => s.Latitude)
                      .HasColumnType("float")
                      .IsRequired(false);

                entity.Property(s => s.Longitude)
                      .HasColumnType("float")
                      .IsRequired(false);

                entity.Property(s => s.IsDeleted)
                      .HasDefaultValue(false);

                entity.Property(s => s.CreatedAt)
                      .HasColumnType("datetime");

                entity.Property(s => s.UpdatedAt)
                      .HasColumnType("datetime");

                entity.HasOne(s => s.District)
                      .WithMany(d => d.Spots)
                      .HasForeignKey(s => s.DistrictId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AgencyService>().ToTable("AgencyService");
            modelBuilder.Entity<AgencyService>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(s => s.Color)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(s => s.IsDeleted)
                      .HasDefaultValue(false);

                entity.Property(s => s.CreatedAt)
                      .HasColumnType("datetime");

                entity.Property(s => s.UpdatedAt)
                      .HasColumnType("datetime");
            });

            modelBuilder.Entity<Booking>().ToTable("Booking");
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(b => b.Id);

                entity.Property(b => b.BookingDate)
                      .HasColumnType("date");

                entity.Property(b => b.StartTime)
                      .HasColumnType("datetime");

                entity.Property(b => b.EndTime)
                      .HasColumnType("datetime");

                entity.Property(b => b.TotalPrice)
                      .HasColumnType("decimal(18,2)");

                entity.Property(b => b.Notes)
                      .HasMaxLength(500);

                entity.Property(b => b.Status)
                      .IsRequired();

                entity.Property(b => b.IsDeleted)
                      .HasDefaultValue(false);

                entity.Property(b => b.CreatedAt)
                      .HasColumnType("datetime");

                entity.Property(b => b.UpdatedAt)
                      .HasColumnType("datetime");

                entity.HasOne(b => b.Customer)
                      .WithMany()
                      .HasForeignKey(b => b.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.Agency)
                      .WithMany()
                      .HasForeignKey(b => b.AgencyId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.Service)
                      .WithMany()
                      .HasForeignKey(b => b.ServiceId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Transaction>().ToTable("Transaction");
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.TransactionCode)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(t => t.Amount)
                      .HasColumnType("decimal(10,2)");

                entity.Property(t => t.PaymentType)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(t => t.CreatedAt)
                      .HasColumnType("datetime");

                entity.Property(t => t.IsDeleted)
                      .HasDefaultValue(false);

                entity.Property(t => t.UpdatedAt)
                      .HasColumnType("datetime");

                entity.HasOne(t => t.SellerPackage)
                      .WithMany(sp => sp.Transactions)
                      .HasForeignKey(t => t.SellerPackageId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Company)
                      .WithMany(c => c.Transactions)
                      .HasForeignKey(t => t.CompanyId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = new Guid("1A73F130-B445-4F46-8F88-D4E4A4645E5C"), Name = RoleEnum.Admin.ToString(), CreatedAt = new DateTime(2024, 1, 1), UpdatedAt = new DateTime(2024, 1, 1), IsDeleted = false },
                new Role { Id = new Guid("2B83F131-B445-4F46-8F88-D4E4A4645E5C"), Name = RoleEnum.ThirdParty.ToString(), CreatedAt = new DateTime(2024, 1, 1), UpdatedAt = new DateTime(2024, 1, 1), IsDeleted = false },
                new Role { Id = new Guid("3C93F132-B445-4F46-8F88-D4E4A4645E5C"), Name = RoleEnum.User.ToString(), CreatedAt = new DateTime(2024, 1, 1), UpdatedAt = new DateTime(2024, 1, 1), IsDeleted = false }
            );
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is IBaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (BaseEntity<Guid>)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(BaseEntity<Guid>.CreatedAt)).IsModified = false;

                    entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
