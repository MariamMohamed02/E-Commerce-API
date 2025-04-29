using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class UnauthorizedException(string msg="Invalid email or password"):Exception(msg)
    {

    }
}
