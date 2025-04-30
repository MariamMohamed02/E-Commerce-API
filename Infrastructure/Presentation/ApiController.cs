using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;

namespace Presentation
{

    [ApiController]
    [Route("api/[controller]")]

    // for swagger to display all scenarios and not only the 200 ok
    // Exceptions are constant across all
    [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ValidationErrorResponse), (int)HttpStatusCode.BadRequest)]
    public class ApiController:ControllerBase
    {
    }
}
