using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.OrderEntities;

namespace Persistance.Data.Configurations
{
    public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(o=>o.Price).HasColumnType("decimal(18,3)");
            // one to one relationship mapped in same table
            builder.OwnsOne(o => o.Product, p => p.WithOwner());
        }
    }
}
