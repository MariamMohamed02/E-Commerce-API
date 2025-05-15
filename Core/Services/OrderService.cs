using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.OrderEntities;
using Domain.Exceptions;
using Services.Specifications;
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
            var shippingAddress =mapper.Map<ShippingAddress>(request.ShipToAddress );
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
            var orderRepo = unitOfWork.GetRepository<Order, Guid>();
            var existingOrder = await orderRepo.GetByIdAsync(new OrderWithPaymentIntentIdSpecifications(basket.PaymentIntentId));
            // make sure the order is not repeated more than once
            if (existingOrder != null) {
                orderRepo.Delete(existingOrder);

            
            }
            var subTotal = orderItems.Sum(item=>item.Price*item.Quantity);

            //Create order
            var order = new Order(userEmail, shippingAddress, orderItems, deliveryMethod, subTotal, basket.PaymentIntentId);

            //Save database
            await orderRepo.AddAsync(order);
            await unitOfWork.SaveChangesAsync();

            // Map, return
            return mapper.Map<OrderResult>(order);
                
        }

        private OrderItem CreateOrderItem(BasketItem item, Product product)
        {
            var productInOrderItem = new ProductInOrderItem(product.Id,product.Name,product.PictureUrl);
            return new OrderItem(productInOrderItem, item.Quantity, product.Price);
        }

        public async Task<IEnumerable<OrderResult>> GetAllOrdersByEmailAsync(string userEmail)
        {
            var orders = await unitOfWork.GetRepository<Order, Guid>().GetAllAsync(new OrderWithIncludesSpecifications(userEmail));
            return mapper.Map<IEnumerable<OrderResult>>(orders);
        }

        public async Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync()
        {
            var methods= await unitOfWork.GetRepository<DeliveryMethod,int>().GetAllAsync();
            return mapper.Map<IEnumerable<DeliveryMethodResult>>(methods);
        }

        public async Task<OrderResult> GetOrderByIdAsync(Guid id)
        {
            var order = await unitOfWork.GetRepository<Order,Guid>().GetByIdAsync(new OrderWithIncludesSpecifications(id))?? throw new OrderNotFoundException(id);
            return mapper.Map<OrderResult>(order);
        }
    }
}
