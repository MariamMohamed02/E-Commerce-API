using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Dtos;

namespace Services.MappingProfiles
{
    public class BasketProfile: Profile
    {
        public BasketProfile() { 
            CreateMap<CustomerBasket,BasketDto>().ReverseMap();
            // MAp BasketItem to BasketItemDto
            CreateMap<BasketItem,BasketItemDto>().ReverseMap();

        }
    }
}
