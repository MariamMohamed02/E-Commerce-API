using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.OrderModels;

namespace Presentation
{
    public class OrdersController(IServiceManager serviceManager):ApiController
    {
        [HttpPost]
        public async Task<ActionResult> Create (OrderRequest orderRequest)
        {
            // get the email from the token
            var email=User.FindFirstValue(ClaimTypes.Email);
            var order = await serviceManager.OrderService.CreateOrderAsync(orderRequest,email);
            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResult>>> GetAllOrders()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var orders= await serviceManager.OrderService.GetAllOrdersByEmailAsync(email);  
            return Ok(orders);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResult>> GetOrder(Guid id)
        {
            var order= await serviceManager.OrderService.GetOrderByIdAsync(id); 
            return Ok(order);
        }

        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodResult>>> GetDeliveryMethods()
        {
            return Ok(await serviceManager.OrderService.GetDeliveryMethodsAsync());
        }


    }
}
