using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.OrderEntities;
using Shared.OrderModels;
using ShippingAddress = Domain.Entities.OrderEntities.Address;
using Address = Domain.Entities.Address;

namespace Services.MappingProfiles
{
    public class OrderProfile: Profile
    {
       public OrderProfile() {

            //  follow that order

            CreateMap<ShippingAddress, AddressDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();   
            CreateMap<DeliveryMethod,DeliveryMethodResult>();   

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d=>d.ProductName, options=>options.MapFrom(s=>s.Product.ProductName))
                .ForMember(d => d.ProductId, options => options.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.PictureUrl, options => options.MapFrom(s => s.Product.PictureUrl));

            CreateMap<Order, OrderResult>()
                .ForMember(d=>d.PaymentStatus, options=>options.MapFrom(s=>s.ToString()))
                .ForMember(d=>d.DeliveryMethod,options=>options.MapFrom(s=>s.DeliveryMethod.ShortName))
                .ForMember(d=>d.Total, options=>options.MapFrom(s=>s.Subtotal+s.DeliveryMethod.Price));
        }
    }
}
