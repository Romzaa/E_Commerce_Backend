using AutoMapper;
using E_Commerce.Application.Contracts;
using E_Commerce.Domain.Contracts;
using E_Commerce.Infrastucture.Data;
using E_Commerce.Infrastucture.DataSeed;
using E_Commerce.Infrastucture.Identity.Data;
using E_Commerce.Infrastucture.Identity.Entities;
using E_Commerce.Infrastucture.Identity.Services;
using E_Commerce.Infrastucture.Repositories;
using Microsoft.AspNetCore.Identity;
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

            services.AddDbContext<StoreIdentityDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));

            services.AddKeyedScoped<IDataSeeder, CatalogDataSeed>("Catalog");
            services.AddKeyedScoped<IDataSeeder, IdentityDataSeed>("Identity");
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICacheRepository, CacheRepository>();

            services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!);
            });

            services.AddIdentityCore<ApplicationUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<StoreIdentityDbContext>();
            services.AddScoped<IIdentityService, IdentityService>();

            return services;
                      
        }
    }
}
