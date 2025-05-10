using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.OrderEntities;

namespace Services.Specifications
{
    internal class OrderWithPaymentIntentIdSpecifications: Specifications<Order>
    {
        public OrderWithPaymentIntentIdSpecifications(string paymentIntentId): base(o=>o.PaymentIntentId == paymentIntentId)
        {

        }
    }
}
