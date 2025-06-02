using Snapspot.Application.Extensions;
using Snapspot.Application.Repositories;
using Snapspot.Application.Services;
using Snapspot.Infrastructure.Extensions;
using Snapspot.Infrastructure.Repositories;
using Snapspot.Infrastructure.Services;
using Snapspot.WebAPI.Extensions;
using Snapspot.WebAPI.Middlewares;

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
            builder.Services.AddScoped<IAgencyService, AgencyService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                _ = app.UseSwagger();
                _ = app.UseSwaggerUI();
            }

            _ = app.UseValidationExceptionMiddleware();

            _ = app.UseHttpsRedirection();

            _ = app.UseAuthentication();

            _ = app.MapControllers();

            app.Run();
        }
    }
}
