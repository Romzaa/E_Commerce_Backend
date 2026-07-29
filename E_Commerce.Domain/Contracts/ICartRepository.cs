using E_Commerce.Domain.Entities.Carts;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Domain.Contracts
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartAsync(string cartId, CancellationToken ct = default);
        Task<Cart?> CreateOrUpdateCartAsynce(Cart cart, TimeSpan? timeToLive = default, CancellationToken ct = default);
        Task<bool> DeleteCartAsync(string cartId, CancellationToken Ct = default);
    }
}
