
using Domain.Contracts;
using E_Commerce.Extensions;
using E_Commerce.Factories;
using E_Commerce.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Persistance.Data;
using Persistance.Data.DataSeeding;
using Persistance.Repositories;
using Services;
using Services.Abstraction;

namespace E_Commerce
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            #region Services
            var builder = WebApplication.CreateBuilder(args);

            // Presentation Services
            builder.Services.AddPresentationServices();


            // Infrastructure Services: 
            builder.Services.AddInfrastructureServices(builder.Configuration);

            // Core Serivices:
            builder.Services.AddCoreServices(builder.Configuration);


            //............................................................

            var app = builder.Build(); 
            #endregion


            #region Pipeline/Middleware
            // Middleware 
            app.CustomMiddleware();
            await app.SeedDbAsync();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // 6. -- To Use the Images --
            app.UseStaticFiles();


            app.UseHttpsRedirection();

            //Middlewares for security
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();

            #endregion

        }
    }
}
