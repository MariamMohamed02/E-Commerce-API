
using Domain.Contracts;
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
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // -------------modeified here to be able to use the controllers in the entire solutin and not just the API project
            builder.Services.AddControllers().AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            // ADDED HERE ...............................................
            // 1.Connect to Database and allow DI for the DbContext
            builder.Services.AddDbContext<AppDbContext>(
                options => { 
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
            });

            // 2. DI for the dataseeding
            builder.Services.AddScoped<IDbInitializer,DbInitializer>();

            //3. 
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            // 4. 
            builder.Services.AddAutoMapper(typeof(Services.AssemblyReference).Assembly);

            //5. 
            builder.Services.AddScoped<IServiceManager, ServiceManager>();

            //............................................................

            var app = builder.Build();

            // 3. Call the function for dataseeding
            // Main returns 'Task' not void since has async function
            await InitializeDbAsync(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // 6. -- To Use the Images --
            app.UseStaticFiles();


            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();


            // Function to be able to call the dataseeding. before even making any request. INstead of clr creating it
            async Task InitializeDbAsync(WebApplication app)
            {
                using var scope= app.Services.CreateScope();
                var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                await dbInitializer.InitializeAync(); // call the method in the interface/ class you created

            }
        }
    }
}
