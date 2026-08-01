using AutoMapper;
using E_Commerce.Application.Contracts;
using E_Commerce.Domain.Contracts;
using E_Commerce.Infrastucture.Data;
using E_Commerce.Infrastucture.DataSeed;
using E_Commerce.Infrastucture.Identity.Data;
using E_Commerce.Infrastucture.Identity.Entities;
using E_Commerce.Infrastucture.Identity.Services;
using E_Commerce.Infrastucture.Payment;
using E_Commerce.Infrastucture.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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
            services.AddScoped<ITokenService, TokenService>();

            var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>() ?? throw new InvalidOperationException("Jwt Setings Is Not Configured");
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    RequireSignedTokens = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            });

            services.AddScoped<IPaymentGateway, StripePaymentGateway>();

            return services;
                      
        }
    }
}
