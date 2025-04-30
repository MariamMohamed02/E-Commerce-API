using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage ="Email is Required")]
        [EmailAddress]
        public string Email { get; init; }

        [Required(ErrorMessage = "Password is Required")]

        public string Password { get; init; }
        public string? PhoneNumber { get; init; }

        [Required(ErrorMessage = "Username is Required")]

        public string UserName { get; init; }

        [Required(ErrorMessage = "Display NAme is Required")]

        public string DisplayName { get; init; }
    }
}
