using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
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
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            _ = services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

            return services;
        }
    }
}
