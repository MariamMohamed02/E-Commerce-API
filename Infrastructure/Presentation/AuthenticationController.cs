using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.Dtos;

namespace Presentation
{
    public class AuthenticationController(IServiceManager serviceManager): ApiController
    {
        //baseurl/api/ControlleeName/Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserResultDto>> Login (LoginDto loginDto)
        {
            var result =await serviceManager.AuthenticationService.Login (loginDto);
            return Ok(result);
        }

      
        [HttpPost("Register")]
        public async Task<ActionResult<UserResultDto>> Register(RegisterDto registerDto)
        {
            var result = await serviceManager.AuthenticationService.Register(registerDto);

            return Ok(result);
        }

    }
}
