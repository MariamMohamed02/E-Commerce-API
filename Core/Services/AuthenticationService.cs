using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
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

            //396e01cc3b7e4c4d05fb59744e76a5c726d3e9992f49d684332e5d393dc4a0ef45c1663bd95aa49137f2d95d8c2e716cf339b12dc295c0b2f734a0fc75118f05f4dc9d72bd9c382c1ef2ebc0dfd419aac7b72f7540e5c161a29d96ded99fdd6af1740ed295d5792757a8ce302b9dc25e04a1efbda6f43953eaf20c06aed6e5858a08532ade52edb278bc434640dd5910f2de57a623f50a32f81f97b143fa545416ea39b1232617513218a1f8e58faf4f194395c5fc5b03ea4bcf468b04e23cc636bc406893882240cbca1a907e0759acdf1666ead174305fc72429b896f80ea33f2524974614693b7ae3c9976ccb592c158e1b76e8484f04e0331dc3b9f0e19b

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("396e01cc3b7e4c4d05fb59744e76a5c726d3e9992f49d684332e5d393dc4a0ef45c1663bd95aa49137f2d95d8c2e716cf339b12dc295c0b2f734a0fc75118f05f4dc9d72bd9c382c1ef2ebc0dfd419aac7b72f7540e5c161a29d96ded99fdd6af1740ed295d5792757a8ce302b9dc25e04a1efbda6f43953eaf20c06aed6e5858a08532ade52edb278bc434640dd5910f2de57a623f50a32f81f97b143fa545416ea39b1232617513218a1f8e58faf4f194395c5fc5b03ea4bcf468b04e23cc636bc406893882240cbca1a907e0759acdf1666ead174305fc72429b896f80ea33f2524974614693b7ae3c9976ccb592c158e1b76e8484f04e0331dc3b9f0e19b"));
            var signinCreds =new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: "https://localhost:7068/",
                audience: "My Audience",
                claims: claims,
                DateTime.UtcNow.AddDays(30),
                signingCredentials: signinCreds);

            return new JwtSecurityTokenHandler().WriteToken(token);
                
         }

    }
}
