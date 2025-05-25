using Microsoft.EntityFrameworkCore;
using Snapspot.Domain.Base;
using Snapspot.Domain.Entities;
using Snapspot.Shared.Enums;

namespace Snapspot.Infrastructure.Persistence.DBContext
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = "Data Source=localhost;Initial Catalog=SnapSpotDB;User ID=sa;Password=12345;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
            base.OnModelCreating(modelBuilder);

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
                      .HasMaxLength(50);

                entity.Property(u => u.Dob)
                      .HasColumnType("date");

                entity.Property(u => u.PhoneNumber)
                      .HasMaxLength(20);

                entity.Property(u => u.AvartaUrl)
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
            });

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = Guid.NewGuid(), Name = RoleEnum.Admin.ToString(), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, IsDeleted = false },
                new Role { Id = Guid.NewGuid(), Name = RoleEnum.ThirdParty.ToString(), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, IsDeleted = false },
                new Role { Id = Guid.NewGuid(), Name = RoleEnum.User.ToString(), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, IsDeleted = false }
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
