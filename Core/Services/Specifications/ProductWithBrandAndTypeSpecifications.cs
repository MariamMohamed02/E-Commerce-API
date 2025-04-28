using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class ProductWithBrandAndTypeSpecifications : Specifications<Product>
    {
        //use to retrieve product by id (where id, include brand/type)
        // filter products by the id then include the brand and type
        public ProductWithBrandAndTypeSpecifications(int id) : base(product => product.Id == id)
        {

            AddInclude(product => product.ProductBrand);
            AddInclude(product => product.ProductType);

        }



        // use to retrieve all products   (include brand/types)

        public ProductWithBrandAndTypeSpecifications(): base(null)
        {
            AddInclude(product => product.ProductBrand);
            AddInclude(product => product.ProductType);
        }

    }
}
