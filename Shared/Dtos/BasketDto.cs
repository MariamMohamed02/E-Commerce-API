using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public  record BasketDto
    {

        // init -> to make the record immutable
        public string Id { get; init; }
        public IEnumerable<BasketItemDto> Items { get; init; }

        // For Payment
        public string? PaymentIntentId { get; init; }
        public string? ClientSecret { get; init; }
        public decimal? ShippingPrice { get; init; }
        public int? DeliveryMethodId { get; init; }


    }
}
