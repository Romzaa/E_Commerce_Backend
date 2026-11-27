using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Domain.Entities.Orders
{
    public class OrderItem : BaseEntity<int>
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public ProductItemOrdered Product { get; set; } = default!;
    }
}
