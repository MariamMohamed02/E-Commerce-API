using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.OrderEntities;

namespace Persistance.Data.Configurations
{
    internal class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, s => s.WithOwner());
            builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);

            // send to db as string and return from it as an enum
            builder.Property(o => o.PaymentStatus).HasConversion(paymentStatus => paymentStatus.ToString(),
                                                                  s=>Enum.Parse<OrderPaymentStatus>(s));

            builder.HasOne(o => o.DeliveryMethod).WithMany().OnDelete(DeleteBehavior.SetNull)  ;
            builder.Property(o => o.Subtotal).HasColumnType("decimal(18,3)");

        }
    }
}
