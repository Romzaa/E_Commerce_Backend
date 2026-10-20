using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Product;
using E_Commerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Profiles
{
    public class ProductThumbnailResolver : IValueResolver<Product, ProductDTO, string>
    {
        private readonly UrlSetting _settings;
        public ProductThumbnailResolver(IOptions<UrlSetting> options)
        {
            _settings = options.Value;
        }
        public string Resolve(Product source, ProductDTO destination, string destMember, ResolutionContext context)
        {
            var baseUrl = _settings.BaseUrl.TrimEnd('/');
            var path = source.ThumbnailUrl.TrimStart('/');
            var fullpath = $"{baseUrl}/Files/{path}";
            return fullpath;
        }

    }

    public class UrlSetting
    {
        public string BaseUrl { get; set; } = default!;
    }
}
