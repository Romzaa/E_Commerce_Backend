using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.Products;
using E_Commerce.Infrastucture.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace E_Commerce.Infrastucture.DataSeed
{
    internal class CatalogDataSeed : IDataSeeder
    {
        private readonly StoreDbContext _dbContext;
        private readonly ILogger _logger;

        public CatalogDataSeed(StoreDbContext dbContext, ILogger<CatalogDataSeed> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            try 
            {
                var pending = await _dbContext.Database.GetPendingMigrationsAsync(ct);
                if (pending.Any())
                    await _dbContext.Database.MigrateAsync(ct);

                var seedRoot = Path.Combine(AppContext.BaseDirectory, "DataSeed");
                await SeedIfEmptyAsync<ProductBrand, int>(seedRoot, "brands.json", ct);
                await SeedIfEmptyAsync<ProductType, int>(seedRoot, "types.json", ct);
                await SeedIfEmptyAsync<Product, int>(seedRoot, "products.json", ct);

                var result = await _dbContext.SaveChangesAsync(ct);
                if(result > 0) 
                { _logger.LogInformation($"{result} Rows Added"); }
                else { 
                _logger.LogInformation("Database Already Seeded");
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An Error Occured While Data Seeding");
            }
        }

        private async Task SeedIfEmptyAsync<T,TKey>(string rootPath , string fileName, CancellationToken ct = default) where T : BaseEntity<TKey>
        {
            if(await _dbContext.Set<T>().AnyAsync(ct))
            {
                _logger.LogInformation("Table Already Has Data");
                return;
            }
            var filePath = Path.Combine(rootPath, fileName);
            if (!File.Exists(filePath)) 
            {
                _logger.LogWarning($"File {fileName} Doesn't Exist");
            
            }

            using var fileStream = File.OpenRead(filePath);

            var options = new JsonSerializerOptions
            {

                PropertyNameCaseInsensitive = true
            };
            var items = await JsonSerializer.DeserializeAsync<List<T>>(fileStream, options , ct);
            if (items?.Any() ?? false)
            {
                _dbContext.Set<T>().AddRange(items);
            }

        }
    }
}
