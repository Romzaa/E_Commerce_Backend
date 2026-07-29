using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Domain.Entities.Carts
{
    public class Cart
    {
        public string Id { get; set; } = default!;
        public ICollection<CartItem> CartItems { get; set; } = [];
    }
}
