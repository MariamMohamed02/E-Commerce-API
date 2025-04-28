using System.ComponentModel;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;

namespace E_Commerce.Factories
{
    public class ApiResponseFactory
    {
        //we need context to be able to reach modelstate which contains the errors and values we want
        public static IActionResult CustomValidationErrorResponse(ActionContext context)
        {
            //1. get all errors in modelstate entry
            //2.Create custom response
            var errors = context.ModelState.Where(error => error.Value.Errors.Any()).Select(error=>
            new ValidationError
            {
                Field = error.Key,
                Errors=error.Value.Errors.Select(e=>e.ErrorMessage)
            }
            );

            var response = new ValidationErrorResponse()
            {
                Errors = errors,
                StatusCode = (int) HttpStatusCode.BadRequest,
                ErrorMessage="Validation Failed"

            };

            return new BadRequestObjectResult(response);
                

        }
    }
}
