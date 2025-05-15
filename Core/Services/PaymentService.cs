using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.OrderEntities;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Services.Specifications;
using Shared.Dtos;
using Stripe;
using Order= Domain.Entities.OrderEntities.Order;
using Product = Domain.Entities.Product;

namespace Services
{
    public class PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration) : IPaymentService
    {
        // Steps:

        // 1. Setup stripe API key (secret key) from the appsettings.json
        // 2. Get the basket (to know the price u need)
        // 3. Basket.Item = Product.Price (to make sure you are using the actual price and not the changed one from the inspect if it happened)
        // 4. Get deliveryMethod and shippingPrice
        // 5. Get shipping price from the database too for the same reason in (3)
        // 6. Calculate total= subtotal + shippingPrice
        // 7. Create or Update paymentIntent with stripe
        // 8. Save Changes to the basket
        // 9. Mapping Basket to Basketdto to return it


        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            StripeConfiguration.ApiKey = configuration.GetSection("StripeSettings")["SecretKey"];
            var basket = await basketRepository.GetBasketAsync(basketId)?? throw new BasketNotFoundException(basketId);
            foreach (var item in basket.Items) {
                var product = await unitOfWork.GetRepository<Product,int>().GetByIdAsync(item.Id)
                    ?? throw new ProductNotFoundException(item.Id);

                item.Price = product.Price;
                
            }

            if (!basket.DeliveryMethodId.HasValue) throw new Exception("No delivery method was selected");
            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod,int>().GetByIdAsync(basket.DeliveryMethodId.Value)
                ?? throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);

            basket.ShippingPrice=deliveryMethod.Price;

            // long -> dollar -> cent
            var amount = (long)(basket.Items.Sum(i => i.Price * i.Quantity) + basket.ShippingPrice) * 100;

            var service = new PaymentIntentService();
            // check if want to create or update
            if (string.IsNullOrEmpty(basket.PaymentIntentId)) {
                //create
                var createOptions = new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = "USD",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                var paymentIntent= await service.CreateAsync(createOptions);  // id, clientsecret
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            
            }
            else
            {
                //update 
                // cases:
                // 1. produt in the cart can have a changed price (by the admin)
                // 2. user can change the deliverymethod
                // 3. user can remove/add items from the basket 


                var updateOptions = new PaymentIntentUpdateOptions
                {
                    Amount = amount
                };
                await service.UpdateAsync(basket.PaymentIntentId, updateOptions);

            }

            await basketRepository.UpdateBasket(basket);
            return mapper.Map<BasketDto>(basket);


        }

        public async Task UpdatePaymentStatus(string request, string stripeHeaders)
        {
            
                var endpointSecret = configuration.GetSection("StripeSettings")["EndPointSecret"];
                var stripeEvent = EventUtility.ConstructEvent(request,
                    stripeHeaders, endpointSecret, throwOnApiVersionMismatch:false);
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                

                // Handle the event
                // If on SDK version < 46, use class Events instead of EventTypes
                if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
                {
                await UpdatePaymentStatusSucceeded(paymentIntent.Id);
                    
                   // Console.WriteLine("PaymentIntent was successful!");
                }
                else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
                {
                await UpdatePaymentStatusFailed(paymentIntent.Id);

                // Console.WriteLine("PaymentMethod was attached to a Customer!");
            }
            // ... handle other event types
            else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

              
            
           
        }

        private async Task UpdatePaymentStatusFailed(string paymentIntentId)
        {

            var orderRepo = unitOfWork.GetRepository<Order, Guid>();
            var order = await orderRepo.GetByIdAsync(new OrderWithPaymentIntentIdSpecifications(paymentIntentId));
            order.PaymentStatus = OrderPaymentStatus.PaymentFailed;
            orderRepo.Update(order);
            await unitOfWork.SaveChangesAsync();
        }

        private async Task UpdatePaymentStatusSucceeded(string paymentIntentId)
        {
            var orderRepo=unitOfWork.GetRepository<Order,Guid>();
            var order = await orderRepo.GetByIdAsync(new OrderWithPaymentIntentIdSpecifications(paymentIntentId));
            order.PaymentStatus= OrderPaymentStatus.PaymentRecieved;
            orderRepo.Update(order);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
