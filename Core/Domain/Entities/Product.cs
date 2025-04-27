using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product:BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price{ get; set; }

        //Navigational Property [One]
        public ProductBrand ProductBrand { get; set; }
        // FK
        public int BrandId { get; set; }

        //Navigational Property [One]

        public ProductType ProductType { get; set; }
        //FK
        public int TypeId { get; set; }

    }
}
