﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.OrderModels
{
    public record OrderResult
    {

        public Guid Id { get; init; }
        public string UserEmail { get; init; }

        public AddressDto ShippingAddress { get; init; }

        public ICollection<OrderItemDto> OrderItems { get; init; } = new List<OrderItemDto>();

        // string since it is an enum
        public string PaymentStatus { get; init; } 
        public string DeliveryMethod { get; init; }
        public int? DeliveryMethodId { get; init; }

        public decimal Subtotal { get; init; } // orderitem price * order item quantity

        public DateTimeOffset OrderDate { get; init; } = DateTimeOffset.Now;
        public string PaymentIntentId { get; init; } = string.Empty;


        public decimal Total { get; init; }
    }
}
