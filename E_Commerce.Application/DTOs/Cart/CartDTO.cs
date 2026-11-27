using E_Commerce.Domain.Entities.Carts;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.DTOs.Cart
{
    public class CartDTO
    {
        public string Id { get; set; } = default!;
        public ICollection<CartItemDTO> CartItems { get; set; } = [];
        public string? ClientSecret { get; set; } = default!;
        public string? PaymentIntentId { get; set; } = default!;
        public int DeliveryMethodId { get; set; } 
        public decimal? ShippingPrice { get; set; } = default!;

    }
}
