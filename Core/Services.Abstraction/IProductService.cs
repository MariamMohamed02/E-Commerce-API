using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;
using Shared.Dtos;

namespace Services.Abstraction
{
    public interface IProductService
    {
        //Get all products
        public Task<PaginatedResult<ProductResultDto>> GetAllProductsAsync(ProductParametersSpecification parameters);
        // Get all brands
        public Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();
        // Get all types
        public Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();
        // Get product by Id
        public Task<ProductResultDto> GetProductByIdAsync(int id);



    }
}
