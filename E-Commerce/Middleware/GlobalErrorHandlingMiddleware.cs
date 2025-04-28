using System.Net;
using System.Runtime.Serialization;
using System.Text.Json;
using Domain.Exceptions;
using Shared.ErrorModels;

namespace E_Commerce.Middleware
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate ?_next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger) {
            _next = next;
            _logger = logger;
        }

        // Response must contain: StatusCode , ErrorMSg
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex) {
                //log exception
                _logger.LogError($"Something went wring: {ex}");
                //handle exception
                await HandleExceptionAsync(httpContext, ex);
            }

        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            //set content type [application/json]
            httpContext.Response.ContentType = "application/json";

            //set status code to 500
            httpContext.Response.StatusCode=(int)HttpStatusCode.InternalServerError;  //500
            httpContext.Response.StatusCode = ex switch
            {
                NotFoundException => (int) HttpStatusCode.NotFound,   //404
                _ => (int)HttpStatusCode.InternalServerError  //500
            };

            //return standard response
            var response = new ErrorDetails
            {
                StatusCode = httpContext.Response.StatusCode,
                ErrorMessage = ex.Message,
            }.ToString();

            // Serialization-> to convert from c# object to json (override the tostring in the errordetails)
           
            await httpContext.Response.WriteAsync(response);

        }
    }
}
