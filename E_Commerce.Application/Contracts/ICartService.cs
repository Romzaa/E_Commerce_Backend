using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Cart;
using E_Commerce.Domain.Entities.Carts;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Contracts
{
    public interface ICartService
    {
        Task<Result<CartDTO>> GetCartAsync(string cartId, CancellationToken ct = default);
        Task<Result<CartDTO>> CreateOrUpdateCartAsync(CartDTO cart, TimeSpan? timeToLive = default, CancellationToken ct =default);
        Task<Result<bool>> DeleteCartAsync(string cartId, CancellationToken ct = default);
    }
}
