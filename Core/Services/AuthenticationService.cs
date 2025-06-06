﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Dtos;
using Shared.OrderModels;

namespace Services
{
    public class AuthenticationService(UserManager<User> _userManager ,IMapper mapper, IOptions<JwtOptions> options ) : IAuthenticationService
    {
        public async Task<bool> CheckIfEmailExist(string email)
        {
           var user = await _userManager.FindByEmailAsync(email);
            return user!=null;
        }

        public async Task<AddressDto> GetUserAddress(string email)
        {
           // var user = await _userManager.FindByEmailAsync(email);
           var user = await _userManager.Users.Include(u=>u.Address)
                .FirstOrDefaultAsync(u=>u.Email==email)?? throw new UserNotFoundException(email);
            return mapper.Map<AddressDto>(user.Address);
        }

        public async Task<UserResultDto> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync (email)?? throw new UserNotFoundException(email);
            return new UserResultDto(user.DisplayName, await CreateTokenAsync(user), user.Email);
        }

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

        public async Task<AddressDto> UpdateUserAddress(AddressDto addressDto, string email)
        {
            var user = await _userManager.Users.Include(u=>u.Address)
                .FirstOrDefaultAsync(u=>u.Email == email)?? throw new UserNotFoundException(email);
            
            if (user.Address != null)  // in case of update
            {
                user.Address.FirstName = addressDto.FirstName;
                user.Address.LastName = addressDto.LastName;
                user.Address.City = addressDto.City;
                user.Address.Country = addressDto.Country;
                user.Address.Street = addressDto.Street;
            }
            else // in case of initial creation
            {
                var address = mapper.Map<Address>(addressDto);
                user.Address = address;
            }

            await _userManager.UpdateAsync(user);
            return mapper.Map<AddressDto>(user.Address);
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
                expires: DateTime.UtcNow.AddDays(jwtOptions.ExpirationInDays),
                signingCredentials: signinCreds);

            return new JwtSecurityTokenHandler().WriteToken(token);
                
         }

    }
}
