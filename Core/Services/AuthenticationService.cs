using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Dtos;

namespace Services
{
    public class AuthenticationService(UserManager<User> _userManager , IOptions<JwtOptions> options ) : IAuthenticationService
    {
        public async Task<UserResultDto> Login(LoginDto loginDto)
        {
            //Email is already added to an account or not (valid or not)

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null) throw new UnauthorizedException();

            // Password correct or no
            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result) throw new UnauthorizedException();
            return new UserResultDto(user.DisplayName, await CreateTokenAsync(user), user.Email);


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
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new ValidationException(errors);
            }



            return new UserResultDto(user.DisplayName, await CreateTokenAsync(user), user.Email);
        }


        private async Task<string> CreateTokenAsync(User user)
        {

            var jwtOptions = options.Value;

            // private claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.Email, user.Email)

            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));

            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));
            var signinCreds =new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                DateTime.UtcNow.AddDays(jwtOptions.ExpirationInDays),
                signingCredentials: signinCreds);

            return new JwtSecurityTokenHandler().WriteToken(token);
                
         }

    }
}
