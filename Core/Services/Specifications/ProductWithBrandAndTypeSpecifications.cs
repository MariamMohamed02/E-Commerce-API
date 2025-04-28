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


        // Sorting
        // Filter based on brandid. productid, both or none
        // To be able to use orderby, it has to be a collection
        // sort by price or by name (asc or desc)  -> 4 cases
        public ProductWithBrandAndTypeSpecifications(string? sort , int? brandId, int? typeId)
            : base(product=>
            (!brandId.HasValue || product.BrandId == brandId.Value) &&
            (!typeId.HasValue || product.TypeId== typeId.Value)
            )
        {
            AddInclude(product => product.ProductBrand);
            AddInclude(product => product.ProductType);
            if (!string.IsNullOrWhiteSpace(sort)) 
            {
                switch (sort.ToLower().Trim())
                {
                    case "pricedesc":
                        SetOrderByDescending(p => p.Price);
                        break;
                    case "priceasc":
                        SetOrderBy(p => p.Price);
                        break;
                    case "namedesc":
                        SetOrderByDescending(p => p.Name);
                        break;
                    default:
                        SetOrderBy(p => p.Name);
                        break;

                }
            }
        }



    }
}
