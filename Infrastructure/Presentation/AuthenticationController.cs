using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.Dtos;
using Shared.OrderModels;

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

        [HttpGet("EmailExist")]
        public async Task<ActionResult<bool>> CheckEmailExist(string email)
        {
            return Ok(await serviceManager.AuthenticationService.CheckIfEmailExist(email));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserResultDto>> GetCurrentUser()
        {
            var email =User.FindFirstValue(ClaimTypes.Email);
            var result = await serviceManager.AuthenticationService.GetUserByEmail(email);  
            return Ok(result);
        }

        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await serviceManager.AuthenticationService.GetUserAddress(email);  
            return Ok(result);
        }

        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto address)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result= await serviceManager.AuthenticationService.UpdateUserAddress(address, email);
            return Ok(result);

        }


    }
}
