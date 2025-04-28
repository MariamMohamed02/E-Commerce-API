using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Shared;

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
        public ProductWithBrandAndTypeSpecifications(ProductParametersSpecification parameters)
            : base(product=>
            (!parameters.BrandId.HasValue || product.BrandId == parameters.BrandId.Value) &&
            (!parameters.TypedId.HasValue || product.TypeId== parameters.TypedId.Value)
            )
        {
            AddInclude(product => product.ProductBrand);
            AddInclude(product => product.ProductType);
            if (parameters.Sort is not null) 
            {
                switch (parameters.Sort)
                {
                    case ProductSortOptions.PriceDesc:
                        SetOrderByDescending(p => p.Price);
                        break;
                    case ProductSortOptions.PriceAsc:
                        SetOrderBy(p => p.Price);
                        break;
                    case ProductSortOptions.NameDesc:
                        SetOrderByDescending(p => p.Name);
                        break;
                    default:
                        SetOrderBy(p => p.Name);
                        break;

                }
            }


            // Pagination
            ApplyPagination(parameters.PageIndex, parameters.PageSize);
        }



    }
}
