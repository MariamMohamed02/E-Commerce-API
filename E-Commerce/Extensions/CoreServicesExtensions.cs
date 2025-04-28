using Services.Abstraction;
using Services;

namespace E_Commerce.Extensions
{
    public static class CoreServicesExtensions
    {

        //builder.services is now - > services
        public static IServiceCollection AddCoreServices(this IServiceCollection services) {

            services.AddAutoMapper(typeof(Services.AssemblyReference).Assembly);
            services.AddScoped<IServiceManager, ServiceManager>();

            return services;
        }
    }
}
