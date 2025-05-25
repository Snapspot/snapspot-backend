using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Snapspot.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Infrastructure.Extensions
{
    public static class ConfigureOptionsExtension
    {
        public static IServiceCollection AddConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
            _ = services.Configure<DatabaseOptions>(configuration.GetSection(nameof(DatabaseOptions)));

            return services;
        }
    }
}
