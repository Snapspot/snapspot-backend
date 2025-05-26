using Microsoft.Extensions.DependencyInjection;
using Snapspot.Application.Repositories;
using Snapspot.Application.Services;
using Snapspot.Infrastructure.Persistence.Repositories;
using Snapspot.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Infrastructure.Extensions
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            _ = services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            _ = services.AddScoped<IUserRepository, UserRepository>();
            _ = services.AddScoped<IJwtService, JwtService>();

            return services;
        }
    }
}
