using AutoMapper;
using E_Commerce.Application.DTOs.Order;
using E_Commerce.Domain.Entities.Orders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Profiles
{
    internal class OrderPictureUrlResolver : IValueResolver<OrderItem, OrderItemDTO, string>
    {
        private readonly UrlSetting _settings;
        public OrderPictureUrlResolver( IOptions<UrlSetting> options)
        {
            _settings = options.Value;
        }
        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {
            var baseUrl = _settings.BaseUrl.TrimEnd('/');
            var pictureUrl = source.Product.PictureUrl?.TrimStart('/');
            var fullpath = $"{baseUrl}/Files/{pictureUrl}";
            return fullpath;
        }
    }
}
