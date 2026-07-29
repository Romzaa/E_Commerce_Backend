using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Carts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace E_Commerce.Infrastucture.Repositories
{
    internal class CartRepository : ICartRepository
    {
        private readonly IDatabase _dataBase;

        public CartRepository(IConnectionMultiplexer connection)
        {
            _dataBase = connection.GetDatabase();
        }
        public async Task<Cart?> CreateOrUpdateCartAsynce(Cart cart, TimeSpan? timeToLive = null , CancellationToken ct = default)
        {
            var value = JsonSerializer.Serialize(cart);
            var result = await _dataBase.StringSetAsync(cart.Id, value, timeToLive ?? TimeSpan.FromDays(7));
            return result ? cart : null;
        }

        public async Task<bool> DeleteCartAsync(string cartId, CancellationToken Ct = default)
        {
            return await _dataBase.KeyDeleteAsync(cartId);
        }

        public async Task<Cart?> GetCartAsync(string cartId, CancellationToken ct = default)
        {
            var cart = await _dataBase.StringGetAsync(cartId);
            return cart.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Cart>(cart.ToString());
        }
    }
}
