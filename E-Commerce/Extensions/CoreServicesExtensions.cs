using Services.Abstraction;
using Services;
using Shared;

namespace E_Commerce.Extensions
{
    public static class CoreServicesExtensions
    {

        //builder.services is now - > services
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration) {

            services.AddAutoMapper(typeof(Services.AssemblyReference).Assembly);
            services.AddScoped<IServiceManager, ServiceManager>();
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
            return services;
        }
    }
}
