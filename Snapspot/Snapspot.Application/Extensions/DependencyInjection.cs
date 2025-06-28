using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Snapspot.Application.UseCases.Implementations.Auth;
using Snapspot.Application.UseCases.Implementations.User;
using Snapspot.Application.UseCases.Implementations.Feedback;
using Snapspot.Application.UseCases.Implementations.Spot;
using Snapspot.Application.UseCases.Implementations.Agency;
using Snapspot.Application.UseCases.Implementations.Transaction;
using Snapspot.Application.UseCases.Implementations.Company;
using Snapspot.Application.UseCases.Implementations.Province;
using Snapspot.Application.UseCases.Implementations.District;
using Snapspot.Application.UseCases.Implementations.SellerPackage;
using Snapspot.Application.UseCases.Implementations.AgencyService;
using Snapspot.Application.UseCases.Interfaces.Auth;
using Snapspot.Application.UseCases.Interfaces.User;
using Snapspot.Application.UseCases.Interfaces.Feedback;
using Snapspot.Application.UseCases.Interfaces.Spot;
using Snapspot.Application.UseCases.Interfaces.Agency;
using Snapspot.Application.UseCases.Interfaces.Transaction;
using Snapspot.Application.UseCases.Interfaces.Company;
using Snapspot.Application.UseCases.Interfaces.Province;
using Snapspot.Application.UseCases.Interfaces.District;
using Snapspot.Application.UseCases.Interfaces.SellerPackage;
using Snapspot.Application.UseCases.Interfaces.AgencyService;
using Snapspot.Application.Validators.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            _ = services.AddScoped<IAuthenticationUseCase, AuthenticationUseCase>();
            _ = services.AddScoped<IUserUseCase, UserUseCase>();
            _ = services.AddScoped<IFeedbackUseCase, FeedbackUseCase>();
            _ = services.AddScoped<ISpotUseCase, SpotUseCase>();
            _ = services.AddScoped<IAgencyUseCase, AgencyUseCase>();
            _ = services.AddScoped<ICompanyUseCase, CompanyUseCase>();
            _ = services.AddScoped<IProvinceUseCase, ProvinceUseCase>();
            _ = services.AddScoped<IDistrictUseCase, DistrictUseCase>();
            _ = services.AddScoped<ISellerPackageUseCase, SellerPackageUseCase>();
            _ = services.AddScoped<IAgencyServiceUseCase, AgencyServiceUseCase>();

            return services;
        }
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            _ = services.AddUseCases();
            _ = services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

            return services;
        }
    }
}
