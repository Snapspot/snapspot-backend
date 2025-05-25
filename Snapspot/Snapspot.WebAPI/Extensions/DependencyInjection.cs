namespace Snapspot.WebAPI.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebAPI(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services.AddCustomSwagger();
            _ = services.AddCustomCors();

            return services;
        }

    }
}
