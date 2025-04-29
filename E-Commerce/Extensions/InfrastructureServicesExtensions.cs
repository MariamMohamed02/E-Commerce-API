using Domain.Contracts;
using Persistance.Data.DataSeeding;
using Persistance.Data;
using Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Persistance.Identity;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;

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

            // Identity Db Conetxt
            services.AddDbContext<IdentityAppDbContext>(
                options => {
                    options.UseSqlServer(configuration.GetConnectionString("IdentityConnectionString"));
                });

            // 2. DI for the dataseeding
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<IdentityAppDbContext>();
                

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
