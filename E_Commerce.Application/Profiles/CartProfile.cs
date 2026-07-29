using AutoMapper;
using E_Commerce.Application.DTOs.Cart;
using E_Commerce.Domain.Entities.Carts;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Profiles
{
    internal class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, CartDTO>().ReverseMap();
            CreateMap<CartItem, CartItemDTO>().ReverseMap();
        }
    }
}
