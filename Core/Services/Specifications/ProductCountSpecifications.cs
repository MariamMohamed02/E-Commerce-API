using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class ProductCountSpecifications: Specifications<Product>
    {
        public ProductCountSpecifications(ProductParametersSpecification parameters)
           : base(product =>
           (!parameters.BrandId.HasValue || product.BrandId == parameters.BrandId.Value) &&
           (!parameters.TypedId.HasValue || product.TypeId == parameters.TypedId.Value)
           )
        {
           


           
        }
    }
}
