using Microsoft.Extensions.DependencyInjection;
using Snapspot.Application.Interfaces;
using Snapspot.Application.Repositories;
using Snapspot.Application.Services;
using Snapspot.Application.Services.Implementations;
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
            _ = services.AddScoped<IUserService, UserService>();

            // Register Booking related services
            _ = services.AddScoped<IBookingRepository, BookingRepository>();
            _ = services.AddScoped<IBookingService, BookingService>();

            // Register Company related services
            _ = services.AddScoped<ICompanyRepository, CompanyRepository>();
            _ = services.AddScoped<ICompanyService, CompanyService>();

            // Register Spot related services
            _ = services.AddScoped<ISpotRepository, SpotRepository>();
            _ = services.AddScoped<ISpotService, SpotService>();

            // Register Province related services
            _ = services.AddScoped<IProvinceRepository, ProvinceRepository>();
            _ = services.AddScoped<IProvinceService, ProvinceService>();

            // Register District related services
            _ = services.AddScoped<IDistrictRepository, DistrictRepository>();
            _ = services.AddScoped<IDistrictService, DistrictService>();

            // Register Agency related services
            _ = services.AddScoped<IAgencyManagementService, AgencyManagementService>();

            //
            _ = services.AddScoped<IAgencyRepository, AgencyRepository>();

            // Register AgencyService related services
            _ = services.AddScoped<IAgencyServiceRepository, AgencyServiceRepository>();
            _ = services.AddScoped<IAgencyServiceService, AgencyServiceService>();

            _ = services.AddScoped<ITransactionService, TransactionService>();
            _ = services.AddScoped<IAppDbContext, AppDbContext>();

            _ = services.AddScoped<ISellerPackageRepository, SellerPackageRepository>();
            _ = services.AddScoped<ISellerPackageService, SellerPackageService>();

            return services;
        }
    }
}
