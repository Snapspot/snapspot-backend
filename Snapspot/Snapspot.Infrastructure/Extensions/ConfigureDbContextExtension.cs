using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Snapspot.Infrastructure.Options;
using Snapspot.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Infrastructure.Extensions
{
    public static class ConfigureDbContextExtension
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseOptions = configuration.GetSection(nameof(DatabaseOptions)).Get<DatabaseOptions>();
            _ = services.AddDbContext<AppDbContext>(options => options.UseSqlServer(databaseOptions.ConnectionStrings));

            return services;
        }
    }
}
