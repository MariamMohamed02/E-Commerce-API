
global using AutoMapper;
global using Domain.Contracts;
global using Services.Abstraction;
global using Shared;
global using Domain.Entities;
using Services.Specifications;
using Shared.Dtos;

namespace Services
{
    // use primary constructor instead of injecting it in the normal constructor
    public class ProductService (IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {
       
        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            //1. Retrieve for all brands -> unit of work
            var brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            // 2. Mapping BrandResultDto ->IMapper
           var brandsResult= _mapper.Map<IEnumerable<BrandResultDto>>(brands);  //IEnumerable <ProductBrand> -> Ienumerable<BrandResultDto>
            // 3. Return 
            return brandsResult;
        }

        public async Task<PaginatedResult<ProductResultDto>> GetAllProductsAsync(ProductSpecificationParameters parameters)
        {
            var products= await _unitOfWork.GetRepository<Product,int>().GetAllAsync(new ProductWithBrandAndTypeSpecifications( parameters));
            var totalCount = await _unitOfWork.GetRepository<Product, int>().CountAsync(new ProductCountSpecifications(parameters));

            var productsResult = _mapper.Map<IEnumerable<ProductResultDto>>(products);
            //return productsResult;

            var result = new PaginatedResult<ProductResultDto>(
                productsResult.Count(),
               // parameters.PageSize,
                parameters.PageIndex,
                totalCount,
                productsResult

                );

            return result;
        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {

            var types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var typesResult = _mapper.Map<IEnumerable<TypeResultDto>>(types);
            return typesResult;
        }

        public async Task<ProductResultDto> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.GetRepository<Product,int>().GetByIdAsync(new ProductWithBrandAndTypeSpecifications(id));
            var productResult = _mapper.Map<ProductResultDto>(product);
            return productResult;
        }
    }
}
