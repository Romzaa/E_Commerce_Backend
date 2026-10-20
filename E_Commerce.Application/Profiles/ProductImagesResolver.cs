using AutoMapper;
using E_Commerce.Application.DTOs.Product;
using E_Commerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;

namespace E_Commerce.Application.Profiles
{
    internal class ProductImagesResolver : IValueResolver<Product, ProductDTO, List<string>>
    {
        private readonly UrlSetting _settings;
        public ProductImagesResolver(IOptions<UrlSetting> options)
        {
            _settings = options.Value;
        }
        public List<string> Resolve(Product source, ProductDTO destination, List<string> destMember, ResolutionContext context)
        {
            var baseUrl = _settings.BaseUrl.TrimEnd('/');
            var paths = new List<string>(source.Images.Count);
            foreach (var image in source.Images)
            {
                var path1 = image.Url.TrimStart('/');
                var fullpath1 = $"{baseUrl}/Files/{path1}";
                paths.Add(fullpath1);
            }

            return paths; ;
        }
    }
}
