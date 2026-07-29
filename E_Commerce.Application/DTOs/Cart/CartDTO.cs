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

    }
}
