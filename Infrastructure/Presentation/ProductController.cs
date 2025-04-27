using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared;

namespace Presentation
{

    [ApiController]
    [Route("/api/[controller]")]
    public class ProductController(IServiceManager _serviceManager): ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResultDto>>> GetAllProducts()
        {
            var products = await _serviceManager.ProductService.GetAllProductsAsync();
            return Ok(products);

        }

        [HttpGet("Brands")]
        public async Task<ActionResult<IEnumerable<BrandResultDto>>> GetAllBrands()
        {
            var brands = await _serviceManager.ProductService.GetAllBrandsAsync();
            return Ok(brands);

        }

        [HttpGet("Types")]
        public async Task<ActionResult<IEnumerable<TypeResultDto>>> GetAllTypes()
        {
            var types = await _serviceManager.ProductService.GetAllTypesAsync();
            return Ok(types);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResultDto>> GetProduct(int id)
        {
            var product=await _serviceManager.ProductService.GetProductByIdAsync(id);
            return Ok(product);
        }



    }
}
