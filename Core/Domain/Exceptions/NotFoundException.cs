using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    // Parent of all not found exceptions in the application
    public class NotFoundException: Exception
    {
        public NotFoundException(string msg): base(msg)
        { 

        
        }
    }
}
