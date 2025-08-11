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
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<SellerPackage> SellerPackages { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Spot> Spots { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<AgencyService> AgencyServices { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<CompanySellerPackage> CompanySellerPackages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<LikePost> LikePosts { get; set; }
        public DbSet<LikeComment> LikeComments { get; set; }
        public DbSet<SavePost> SavePosts { get; set; }
        public DbSet<Style> Styles { get; set; }
        public DbSet<StyleSpot> StyleSpots { get; set; }

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
            });


            modelBuilder.Entity<CompanySellerPackage>(entity =>
            {
                entity.ToTable("CompanySellerPackage");
                    
                entity.HasKey(x => new { x.CompaniesId, x.SellerPackagesId });

                entity.HasOne(x => x.Company)
                      .WithMany(c => c.CompanySellerPackages)
                      .HasForeignKey(x => x.CompaniesId);

                entity.HasOne(x => x.SellerPackage)
                      .WithMany(sp => sp.CompanySellerPackages)
                      .HasForeignKey(x => x.SellerPackagesId);

                entity.Property(x => x.RemainingDay)
                      .IsRequired();

                entity.Property(x => x.IsActive)
                      .HasDefaultValue(true);
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
                      .WithOne(u => u.Company)
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
                      .WithMany(s => s.Agencies)
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
                      .WithMany(u => u.Feedbacks)
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

            // Post
            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Content).HasMaxLength(5000);
                entity.Property(p => p.IsDeleted).HasDefaultValue(false);
                entity.Property(p => p.CreatedAt).HasColumnType("datetime");
                entity.Property(p => p.UpdatedAt).HasColumnType("datetime");
                entity.HasOne(p => p.User)
                      .WithMany(u => u.Posts)
                      .HasForeignKey(p => p.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.Spot)
                      .WithMany(s => s.Posts)
                      .HasForeignKey(p => p.SpotId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(p => p.Images)
                      .WithOne(i => i.Post)
                      .HasForeignKey(i => i.PostId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(p => p.Comments)
                      .WithOne(c => c.Post)
                      .HasForeignKey(c => c.PostId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(p => p.LikePosts)
                      .WithOne(lp => lp.Post)
                      .HasForeignKey(lp => lp.PostId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(p => p.SavePosts)
                      .WithOne(sp => sp.Post)
                      .HasForeignKey(sp => sp.PostId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            // Comment
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Content).IsRequired().HasMaxLength(1000);
                entity.Property(c => c.CreatedAt).HasColumnType("datetime");
                entity.HasOne(c => c.User)
                      .WithMany(u => u.Comments)
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(c => c.Post)
                      .WithMany(p => p.Comments)
                      .HasForeignKey(c => c.PostId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(c => c.LikeComments)
                      .WithOne(lc => lc.Comment)
                      .HasForeignKey(lc => lc.CommentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            // Image
            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("Image");
                entity.HasKey(i => i.Id);
                entity.Property(i => i.Uri).HasMaxLength(500);
                entity.HasOne(i => i.Post)
                      .WithMany(p => p.Images)
                      .HasForeignKey(i => i.PostId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            // LikePost
            modelBuilder.Entity<LikePost>(entity =>
            {
                entity.ToTable("LikePost");
                entity.HasKey(lp => new { lp.UserId, lp.PostId });
                entity.HasOne(lp => lp.User)
                      .WithMany(u => u.LikePosts)
                      .HasForeignKey(lp => lp.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(lp => lp.Post)
                      .WithMany(p => p.LikePosts)
                      .HasForeignKey(lp => lp.PostId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            // LikeComment
            modelBuilder.Entity<LikeComment>(entity =>
            {
                entity.ToTable("LikeComment");
                entity.HasKey(lc => new { lc.UserId, lc.CommentId });
                entity.HasOne(lc => lc.User)
                      .WithMany(u => u.LikeComments)
                      .HasForeignKey(lc => lc.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(lc => lc.Comment)
                      .WithMany(c => c.LikeComments)
                      .HasForeignKey(lc => lc.CommentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            // SavePost
            modelBuilder.Entity<SavePost>(entity =>
            {
                entity.ToTable("SavePost");
                entity.HasKey(sp => new { sp.UserId, sp.PostId });
                entity.HasOne(sp => sp.User)
                      .WithMany(u => u.SavePosts)
                      .HasForeignKey(sp => sp.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(sp => sp.Post)
                      .WithMany(p => p.SavePosts)
                      .HasForeignKey(sp => sp.PostId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            // Style configuration
            modelBuilder.Entity<Style>().ToTable("Style");
            modelBuilder.Entity<Style>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Category)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(s => s.Description)
                      .IsRequired()
                      .HasMaxLength(500);

                entity.Property(s => s.Image)
                      .HasMaxLength(500);

                entity.Property(s => s.IsDeleted)
                      .HasDefaultValue(false);

                entity.Property(s => s.CreatedAt)
                      .HasColumnType("datetime");

                entity.Property(s => s.UpdatedAt)
                      .HasColumnType("datetime");
            });

            // StyleSpot configuration (bảng trung gian)
            modelBuilder.Entity<StyleSpot>().ToTable("StyleSpot");
            modelBuilder.Entity<StyleSpot>(entity =>
            {
                entity.HasKey(ss => ss.Id);

                entity.HasOne(ss => ss.Style)
                      .WithMany(s => s.StyleSpots)
                      .HasForeignKey(ss => ss.StyleId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ss => ss.Spot)
                      .WithMany(s => s.StyleSpots)
                      .HasForeignKey(ss => ss.SpotId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(ss => ss.IsDeleted)
                      .HasDefaultValue(false);

                entity.Property(ss => ss.CreatedAt)
                      .HasColumnType("datetime");

                entity.Property(ss => ss.UpdatedAt)
                      .HasColumnType("datetime");
            });
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
