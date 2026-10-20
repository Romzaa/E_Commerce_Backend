using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace E_Commerce.Infrastucture.Attachment
{
    public class AddProductDto
    {

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = default!;

        [Required]
        [MaxLength(600)]
        public string Description { get; set; } = default!;

        public IFormFile? ThumbnailUrl { get; set; }

        [Range(0, 100)]
        public decimal DiscountPercentage { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [Required]
        public int BrandId { get; set; } = default!;

        [Required]
        public int TypeId { get; set; } = default!;

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public List<IFormFile?> Images { get; set; } = new();
    }
}
