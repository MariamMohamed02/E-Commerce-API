using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public record BasketItemDto
    {

        // init -> to make the record immutable
        public int Id { get; init; }
        public string Name { get; init; }
        public string PictureUrl { get; init; }

        [Range(1,99)]
        public int Quantity { get; init; }

        [Range(1,double.MaxValue)]
        public decimal Price { get; init; }
        //public string Brand { get; init; }
        //public string Category { get; init; }
    }
}
