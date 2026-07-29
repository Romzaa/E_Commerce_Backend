using E_Commerce.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastucture.Data.Configurations
{
    internal class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name)
                    .HasMaxLength(100);
            builder.Property(p => p.Description)
                    .HasMaxLength(500);
            builder.Property(p => p.PictureUrl)
                    .HasMaxLength(200);
            builder.Property(p => p.price)
                    .HasColumnType("decimal(18,2)");

            builder.HasOne(p => p.ProductBrand)
                .WithMany()
                .HasForeignKey(p => p.BrandId);
            builder.HasOne(p => p.ProductType)
                .WithMany()
                .HasForeignKey(p => p.TypeId);
        }
    }
}
