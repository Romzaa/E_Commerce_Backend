using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Infrastucture.Data;
using E_Commerce.Infrastucture.DataSeed;
using E_Commerce.Infrastucture.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastucture
{
    public static class InfraStructureServicesRegistration
    {
        public static IServiceCollection AddInfraStructureServices(this IServiceCollection services, IConfiguration configuration) {

            services.AddDbContext<StoreDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddKeyedScoped<IDataSeeder, CatalogDataSeed>("Catalog");
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICacheRepository, CacheRepository>();

            services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!);
            });

            return services;
                      
        }
    }
}
