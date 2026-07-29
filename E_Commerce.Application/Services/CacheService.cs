using E_Commerce.Application.Contracts;
using E_Commerce.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace E_Commerce.Application.Services
{
    public class CacheService : ICacheService
    {
        private readonly ICacheRepository _cacheRepo;

        public CacheService( ICacheRepository cacheRepo)
        {
            _cacheRepo = cacheRepo;
        }
        public async Task<string?> GetAsync(string cacheKey, CancellationToken ct = default)
        {
            return await _cacheRepo.GetAsync(cacheKey, ct);
        }

        public async Task SetAsync(string cacheKey, object cacheValue, TimeSpan timeToLive, CancellationToken ct = default)
        {
            var jsonValue = JsonSerializer.Serialize(cacheValue, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            await _cacheRepo.SetAsync(cacheKey, jsonValue, timeToLive, ct);
        }
    }
}
