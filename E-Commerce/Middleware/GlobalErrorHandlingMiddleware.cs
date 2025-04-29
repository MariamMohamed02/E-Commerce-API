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
                // Incorrect controller (doesnt throw an exception therefore wont be called from inside the catch therefore write here)
                if (httpContext.Response.StatusCode == (int)HttpStatusCode.NotFound)
                    await HandleNotFoundApiAsync(httpContext);
            }
            catch (Exception ex) {
                //log exception
                _logger.LogError($"Something went wring: {ex}");
                //handle exception
                await HandleExceptionAsync(httpContext, ex);
            }

        }

        private async Task HandleNotFoundApiAsync(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json";
            var response= new ErrorDetails
            {
                StatusCode=(int)HttpStatusCode.NotFound,
                ErrorMessage= $"The EndPoint {httpContext.Request.Path} not correct"
            }.ToString();
            await httpContext.Response.WriteAsync(response);
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            //set content type [application/json]
            httpContext.Response.ContentType = "application/json";

            //set status code to 500
            httpContext.Response.StatusCode=(int)HttpStatusCode.InternalServerError;  //500

            var response = new ErrorDetails
            {
                ErrorMessage = ex.Message,
            };
            httpContext.Response.StatusCode = ex switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,   //404
                UnauthorizedException => (int)HttpStatusCode.Unauthorized,  //401
                ValidationException validationException => HandleValidationException(validationException, response),
                _ => (int)HttpStatusCode.InternalServerError  //500
            };
            response.StatusCode=httpContext.Response.StatusCode;

            //return standard response
            //var response = new ErrorDetails
            //{

            //    StatusCode = httpContext.Response.StatusCode,
            //    ErrorMessage = ex.Message,
            //}.ToString();

            // Serialization-> to convert from c# object to json (override the tostring in the errordetails)
           
            await httpContext.Response.WriteAsync(response.ToString());

        }

        private int HandleValidationException(ValidationException validationException, ErrorDetails response)
        {
            response.Errors=validationException.Errors;
            return (int)HttpStatusCode.BadRequest;
        }
    }
}
