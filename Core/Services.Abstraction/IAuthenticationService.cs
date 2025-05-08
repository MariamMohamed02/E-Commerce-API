using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Dtos;
using Shared.OrderModels;

namespace Services.Abstraction
{
    public interface IAuthenticationService
    {
        // Response returned to frontend: Displayname, email, token
        public Task<UserResultDto> Login(LoginDto loginDto);
        public Task<UserResultDto> Register(RegisterDto registerDto);

        //Get Current User
        public Task<UserResultDto> GetUserByEmail(string email);

        // Check if email exists
        public Task<bool> CheckIfEmailExist(string email);
        //update user address
        public Task<AddressDto> UpdateUserAddress(AddressDto addressDto, string email);

        // get user
        public Task<AddressDto> GetUserAddress(string email);
    }
}
