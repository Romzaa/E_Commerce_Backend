using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Domain.Entities.Products
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string ThumbnailUrl { get; set; } = default!;
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int StockQuantity { get; set; }
        public decimal Rating { get; set; }
        public int BrandId { get; set; }
        public ProductBrand ProductBrand { get; set; } = default!;

        public decimal FinalPrice => DiscountPercentage >0 ? Price - (Price * (DiscountPercentage / 100)) : Price;
        public int TypeId { get; set; }
        public ProductType ProductType { get; set; } = default!;

        public ICollection<Image> Images { get; set; } = default!;
        public ICollection<Review> Reviews { get; set; } = default!;

    }
}
