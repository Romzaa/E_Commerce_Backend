using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Domain.Entities.Carts
{
    public class Cart
    {
        public string Id { get; set; } = default!;
        public ICollection<CartItem> CartItems { get; set; } = [];
        public string? ClientSecret { get; set; } = default!;
        public string? PaymentIntentId { get; set; } = default!;
        public int DeliveryMethodId { get; set; } 
        public decimal? ShippingPrice { get; set; } = default!;
    }
}
