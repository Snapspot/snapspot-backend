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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

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
            // Defines the base route for this controller.
            // [controller] will be replaced by the controller's name (without the 'Controller' suffix).
            // Note: For RESTful best practices, URLs should be lowercase. Consider using explicit lowercase routes or enabling LowercaseUrls in configuration.
            _ = builder.Services.AddRouting(options => options.LowercaseUrls = true);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            _ = builder.Services.AddEndpointsApiExplorer();
            _ = builder.Services.AddSwaggerGen();


            var app = builder.Build();


            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                _ = app.UseSwagger();
                _ = app.UseSwaggerUI();
            //}

            _ = app.UseValidationExceptionMiddleware();

            _ = app.UseHttpsRedirection();

            _ = app.UseCors("AllowFrontend");

            _ = app.UseAuthentication();
            _ = app.UseAuthorization();

            _ = app.MapControllers();

            app.Run();
        }
    }
}