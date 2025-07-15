using Microsoft.Extensions.DependencyInjection;
using Snapspot.Application.Repositories;
using Snapspot.Application.Services;
using Snapspot.Infrastructure.Persistence.DBContext;
using Snapspot.Infrastructure.Persistence.Repositories;
using Snapspot.Infrastructure.Repositories;
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

            // Register Company related services
            _ = services.AddScoped<ICompanyRepository, CompanyRepository>();

            // Register Spot related services
            _ = services.AddScoped<ISpotRepository, SpotRepository>();

            // Register Province related services
            _ = services.AddScoped<IProvinceRepository, ProvinceRepository>();

            // Register District related services
            _ = services.AddScoped<IDistrictRepository, DistrictRepository>();

            // Register Agency related services
            _ = services.AddScoped<IAgencyRepository, AgencyRepository>();

            // Register AgencyService related services
            _ = services.AddScoped<IAgencyServiceRepository, AgencyServiceRepository>();

            // Register Feedback related services
            _ = services.AddScoped<IFeedbackRepository, FeedbackRepository>();

            _ = services.AddScoped<ISellerPackageRepository, SellerPackageRepository>();

            _ = services.AddScoped<ICompanySellerPackageRepository, CompanySellerPackageRepository>();

            return services;
        }
    }
}
