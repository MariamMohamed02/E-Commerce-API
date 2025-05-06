using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.OrderEntities;
using Domain.Exceptions;
using Shared.OrderModels;
using ShippingAddress = Domain.Entities.OrderEntities.Address;


namespace Services
{
    internal class OrderService(IMapper mapper, IBasketRepository basketRepository, IUnitOfWork unitOfWork) : IOrderService
    {
        public async Task<OrderResult> CreateOrderAsync(OrderRequest request, string userEmail)
        {
            // get price from the database since frontend can change it in the inspect

            // Shipping Address
            var shippingAddress =mapper.Map<ShippingAddress>(request.ShippingAddress );
            // OrderItems => Basket{BasketId} => BasketItems => OrderItems
            var basket = await basketRepository.GetBasketAsync(request.BasketId) ?? throw new BasketNotFoundException(request.BasketId);
            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items) { 
            
                var product = await unitOfWork.GetRepository<Product,int>().GetByIdAsync(item.Id)
                    ?? throw new ProductNotFoundException(item.Id);

                orderItems.Add(CreateOrderItem(item, product));
            
            }

            //DeliveryMethod
            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod,int>()
                .GetByIdAsync(request.DeliveryMethodId) ?? throw new DeliveryMethodNotFoundException(request.DeliveryMethodId);

            //Subtotal
            var subTotal = orderItems.Sum(item=>item.Price*item.Quantity);

            //Create order
            var order = new Order(userEmail, shippingAddress, orderItems, deliveryMethod, subTotal);

            //Save database
            await unitOfWork.GetRepository<Order,Guid>().AddAsync(order);
            await unitOfWork.SaveChangesAsync();

            // Map, return
            return mapper.Map<OrderResult>(order);
                
        }

        private OrderItem CreateOrderItem(BasketItem item, Product product)
        {
            var productInOrderItem = new ProductInOrderItem(product.Id,product.Name,product.PictureUrl);
            return new OrderItem(productInOrderItem, item.Quantity, product.Price);
        }

        public Task<IEnumerable<OrderResult>> GetAllOrdersByEmailAsync(string userEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OrderResult> GetOrderByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
