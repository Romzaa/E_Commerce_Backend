using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Cart;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Carts;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Services
{
    internal class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepo
                          ,IMapper mapper)
        {
            _cartRepo = cartRepo;
            _mapper = mapper;
        }
        public async Task<Result<CartDTO>> CreateOrUpdateCartAsync(CartDTO cart, TimeSpan? timeToLive = default, CancellationToken ct = default)
        {
            var customerCart = _mapper.Map<Cart>(cart);
            var result = await _cartRepo.CreateOrUpdateCartAsynce(customerCart, timeToLive, ct);
            return result is null ?
                    Result<CartDTO>.Fail(Error.Failure("CartCreateOrUpdate.Failure", "Failed To Create Or Update Cart"))
                    : Result<CartDTO>.Ok(cart);
        }

        public async Task<Result<bool>> DeleteCartAsync(string cartId, CancellationToken ct = default)
        {
            var result = await _cartRepo.DeleteCartAsync(cartId, ct);
            return result ? Result<bool>.Ok(true) : Result<bool>.Fail(Error.Failure("CartDelete.Failure", "Failed To Delete Cart")); 

        }

        public async Task<Result<CartDTO>> GetCartAsync(string cartId, CancellationToken ct = default)
        {
            var cart = await _cartRepo.GetCartAsync(cartId,ct);
            return cart == null ?
                    Result<CartDTO>.Fail(Error.Failure("Cart.NotFound", $"Cart With Id {cartId} Is Not Found"))
                    : Result<CartDTO>.Ok(_mapper.Map<CartDTO>(cart));
        }
    }
}
