using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.OrderModels;

namespace Services.Abstraction
{
    public interface IOrderService
    {

        //Get Order by Id  -> Takes: Guid id and returns OrderResult type
        Task<OrderResult> GetOrderByIdAsync(Guid id);
        //Get all orders for user by email -> email string and returns Ienumerable<OrderResult>
        Task<IEnumerable<OrderResult>> GetAllOrdersByEmailAsync(string userEmail);
        //Create order  -> takes: OrderRequest and email, returns OrderResult
        Task<OrderResult> CreateOrderAsync(OrderRequest request, string userEmail);
        //Get all delivery Methods -> returns Ienumeearable<DeliveryMethodResult>
        Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync();


    }
}
