using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.OrderEntities;

namespace Services.Specifications
{
    public class OrderWithIncludesSpecifications: Specifications<Order>
    {
        //Get Order by id ; return one object
        public OrderWithIncludesSpecifications(Guid id): base(o=>o.Id==id)
        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o=>o.OrderItems);
        }

        //Get all orders by email; returns collection
        public OrderWithIncludesSpecifications(string email) : base(o => o.UserEmail == email)
        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.OrderItems);
            SetOrderBy(o => o.OrderDate);
        }
    }
}
