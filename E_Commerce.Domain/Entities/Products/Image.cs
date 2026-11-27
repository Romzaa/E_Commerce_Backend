using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Domain.Entities.Products
{
    public class Image : BaseEntity<int>
    {
            public string Url { get; set; } = default!;
            public int ProductId { get; set; }
            public Product Product { get; set; } = default!;
        
    }
}
