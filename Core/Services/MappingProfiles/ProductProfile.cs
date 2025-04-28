using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Shared.Dtos;

namespace Services.MappingProfiles
{
    public class ProductProfile: Profile
    {
        public ProductProfile() {
            CreateMap<ProductBrand, BrandResultDto>();
            CreateMap<ProductType, TypeResultDto>();
            CreateMap<Product,ProductResultDto>()
                .ForMember(d=>d.BrandName, options=>options.MapFrom(s=>s.ProductBrand.Name))
                .ForMember(d => d.TypeName, options => options.MapFrom(s => s.ProductType.Name))
                .ForMember(d=>d.PictureUrl, options =>options.MapFrom<PictureUrlResolver>());

        }
    }
}
