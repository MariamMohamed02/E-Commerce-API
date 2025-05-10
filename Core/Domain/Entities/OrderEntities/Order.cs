using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShippingAddress = Domain.Entities.OrderEntities.Address;

namespace Domain.Entities.OrderEntities
{
    public class Order: BaseEntity<Guid>
    {
        public Order() { }
        public Order(string userEmail, ShippingAddress shippingAddress, ICollection<OrderItem> orderItems, DeliveryMethod deliveryMethod,  decimal subtotal, string paymentIntendId )
        {
            UserEmail = userEmail;
            ShippingAddress = shippingAddress;
            OrderItems = orderItems;
            DeliveryMethod = deliveryMethod;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntendId;



        }

        public string UserEmail { get; set; }

        public ShippingAddress ShippingAddress { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public OrderPaymentStatus PaymentStatus { get; set; } =OrderPaymentStatus.Pending;
        public DeliveryMethod DeliveryMethod { get; set; }
        public int? DeliveryMethodId { get; set; }

        public decimal Subtotal { get; set; } // orderitem price * order item quantity

        public DateTimeOffset OrderDate { get; set; }= DateTimeOffset.Now;
        public string PaymentIntentId { get; set; } 
    }
}
