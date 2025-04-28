using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    //record ->used when you want to compare between two instances based on the value and not the refernce
    // record -> object is immutable

    public record ProductResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }

        public string BrandName { get; set; }
        public string TypeName
        {
            get; set;


        }
    }
}
