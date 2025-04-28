using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Shared.Dtos;

namespace Services.MappingProfiles
{
    internal class PictureUrlResolver (IConfiguration _configuration) : IValueResolver<Product, ProductResultDto, string>
    {
        public string Resolve(Product source, ProductResultDto destination, string destMember, ResolutionContext context)
        {
            //check if image is empty first
            if (string.IsNullOrWhiteSpace(source.PictureUrl)) return string.Empty;
            return $"{_configuration["baseUrl"]}{source.PictureUrl}";
        }
    }
}
