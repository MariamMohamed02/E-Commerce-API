using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.ErrorModels
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }

        public override string ToString()=> JsonSerializer.Serialize(this);

        //Property added for the VAlidationException to show the error messgae
        public IEnumerable<string> Errors { get; set; }

    }
}
