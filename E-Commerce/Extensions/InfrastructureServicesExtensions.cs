using Domain.Contracts;
using Persistance.Data.DataSeeding;
using Persistance.Data;
using Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace E_Commerce.Extensions
{
    public static class InfrastructureServicesExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration) {

            // 1.Connect to Database and allow DI for the DbContext
            services.AddDbContext<AppDbContext>(
                options => {
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"));
                });

            // 2. DI for the dataseeding
            services.AddScoped<IDbInitializer, DbInitializer>();


            //3. 
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //
            services.AddScoped<IBasketRepository, BasketRepository>();

            // Connection MUltiplexer 
            services.AddSingleton<IConnectionMultiplexer>(services => ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!));

            return services;
        }
    }
}
