using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Shared.Dtos;

namespace Services
{
    public class AuthenticationService(UserManager<User> _userManager) : IAuthenticationService
    {
        public async Task<UserResultDto> Login(LoginDto loginDto)
        {
            //Email is already added to an account or not (valid or not)

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null) throw new UnauthorizedException();

            // Password correct or no
            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if(!result) throw new UnauthorizedException();
            return new UserResultDto(user.DisplayName, "Token", user.Email);


        }

        public async Task<UserResultDto> Register(RegisterDto registerDto)
        {
            var user = new User()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.DisplayName,
                PhoneNumber = registerDto.PhoneNumber,
            };
           var result= await _userManager.CreateAsync(user,registerDto.Password);
            return new UserResultDto(user.DisplayName,"Token",user.Email);
        }
    }
}
