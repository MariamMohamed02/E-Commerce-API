using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Dtos;

namespace Services.Abstraction
{
    public interface IAuthenticationService
    {
        // Response returned to frontend: Displayname, email, token
        public Task<UserResultDto> Login(LoginDto loginDto);
        public Task<UserResultDto> Register(RegisterDto registerDto);
    }
}
