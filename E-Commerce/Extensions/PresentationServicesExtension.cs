using E_Commerce.Factories;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Extensions
{
    public static class PresentationServicesExtension
    {

        public static IServiceCollection AddPresentationServices(this IServiceCollection services) {


            // -------------modeified here to be able to use the controllers in the entire solutin and not just the API project
            services.AddControllers().AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            //7. Custom Vaildation of request data
            services.Configure<ApiBehaviorOptions>(options =>
            {
                // func => return IActionResult and takes Action Context as parameter
                options.InvalidModelStateResponseFactory = ApiResponseFactory.CustomValidationErrorResponse;
            });

            return services;
        }
    }
}
