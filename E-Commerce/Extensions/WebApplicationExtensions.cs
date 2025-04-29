using System.Runtime.CompilerServices;
using Domain.Contracts;
using E_Commerce.Middleware;

namespace E_Commerce.Extensions
{
    // For Middlewares
    public static class WebApplicationExtensions
    {
        // Return webapplication in case you later want to chain
        // To be able to Data Seed
        public static async Task <WebApplication >SeedDbAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializeAync(); // call the method in the interface/ class you created
            await dbInitializer.InitializeIdentityAsync();
            return app;
        }

        public static WebApplication CustomMiddleware(this WebApplication app) {

            app.UseMiddleware<GlobalErrorHandlingMiddleware>();
            return app;
        }
    }
}
