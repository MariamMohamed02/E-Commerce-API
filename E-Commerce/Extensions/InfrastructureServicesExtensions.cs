using Domain.Contracts;
using Persistance.Data.DataSeeding;
using Persistance.Data;
using Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Persistance.Identity;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace E_Commerce.Extensions
{
    public static class InfrastructureServicesExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration) {

            // 1.Connect to Database and allow DI for the DbContext
            services.AddDbContext<AppDbContext>(
                options =>
                {
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

            // Configure the token recieved
            services.ConfigureJwt(configuration);

            return services;
        }
    
        public static IServiceCollection ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();
            // validate token
            services.AddAuthentication(options =>
            {
                //validate default scheme
                options.DefaultAuthenticateScheme= JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer= jwtOptions.Issuer,
                    ValidAudience= jwtOptions.Audience,
                    IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                };
            });

            // To make aitherize the 'roles' of the person
            services.AddAuthorization();

            return services;
            
        }
    
    }
}
