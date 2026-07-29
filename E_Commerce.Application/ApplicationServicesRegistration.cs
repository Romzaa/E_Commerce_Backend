using E_Commerce.Application.Contracts;
using E_Commerce.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices( this IServiceCollection services)
        {
            services.AddAutoMapper(c => { }, typeof(ApplicationServicesRegistration).Assembly);
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICacheService, CacheService>();
            return services;

        }

    }
}
