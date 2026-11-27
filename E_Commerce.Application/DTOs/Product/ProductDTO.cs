using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.DTOs.Product
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string ThumbnailUrl { get; set; } = default!;
        public decimal DiscountPercentage { get; set; }
        public int StockQuantity { get; set; }
        public decimal Rating { get; set; }
        public string ProductBrand { get; set; } = default!;
        public string ProductType { get; set; } = default!;
        public decimal Price { get; set; }
        public decimal FinalPrice { get; set; }
        public List<string> Images { get; set; } = default!;
        public List<ReviewDTO> Reviews { get; set; } = default!;
    }
}
