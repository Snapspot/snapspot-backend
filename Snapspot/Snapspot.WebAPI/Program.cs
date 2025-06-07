using Snapspot.Application.Extensions;
using Snapspot.Application.Repositories;
using Snapspot.Application.Services;
using Snapspot.Infrastructure.Extensions;
using Snapspot.Infrastructure.Persistence.DBContext;
using Snapspot.Infrastructure.Persistence.Seed;
using Snapspot.Infrastructure.Repositories;
using Snapspot.Infrastructure.Services;
using Snapspot.WebAPI.Extensions;
using Snapspot.WebAPI.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace Snapspot.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            _ = builder.Services.AddApplication();
            _ = builder.Services.AddInfrastructure(builder.Configuration);
            _ = builder.Services.AddWebAPI(builder.Configuration);

            _ = builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            _ = builder.Services.AddEndpointsApiExplorer();
            _ = builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IAgencyRepository, AgencyRepository>();
            builder.Services.AddScoped<IAgencyManagementService, AgencyManagementService>();

            var app = builder.Build();

            // Apply migrations and seed data
            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    
                    // Ensure database is created
                    context.Database.EnsureCreated();
                    
                    // Apply any pending migrations
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        context.Database.Migrate();
                    }
                    
                    // Seed data
                    UserRoleSeeder.SeedData(context);
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                    throw;
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                _ = app.UseSwagger();
                _ = app.UseSwaggerUI();
            }

            _ = app.UseValidationExceptionMiddleware();

            _ = app.UseHttpsRedirection();

            _ = app.UseAuthentication();
            _ = app.UseAuthorization();

            _ = app.MapControllers();

            app.Run();
        }
    }
}
