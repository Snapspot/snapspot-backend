using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Snapspot.Infrastructure.Options;
using Snapspot.Infrastructure.Services;
using Snapspot.Infrastructure.Repositories;
using Snapspot.Application.Services;
using Snapspot.Application.Repositories;
using Snapspot.Application.UseCases.Interfaces.Transaction;
using Snapspot.Application.UseCases.Implementations.Transaction;

namespace Snapspot.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services.AddConfigureOptions(configuration);
            _ = services.AddCustomDbContext(configuration);
            _ = services.AddCustomJwt(configuration);
            _ = services.AddServices();
            services.Configure<PayOSOptions>(configuration.GetSection("PayOS"));
            services.AddHttpClient<IPayOSService, PayOSService>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITransactionUseCase, TransactionUseCase>();
            return services;
        }

    }
}
