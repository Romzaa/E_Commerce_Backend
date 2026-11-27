using E_Commerce.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastucture.Data.Configurations
{
    internal class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.Property(o => o.SubTotal).HasColumnType("decimal(8,2)");
            builder.Property(o => o.BuyerEmail).IsRequired().HasMaxLength(256);
            builder.Property(o => o.Status).HasConversion<string>().HasMaxLength(50);

            builder.HasMany(o => o.Items)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.DeliveryMethod)
                    .WithMany()
                    .HasForeignKey(o => o.DeliverMethodId);

            builder.OwnsOne(o => o.ShipToAddress);
        }
    }
}
