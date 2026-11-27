using AutoMapper;
using E_Commerce.Application.DTOs;
using E_Commerce.Application.DTOs.Order;
using E_Commerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Profiles
{
    internal class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderAddress, AddressDTO>().ReverseMap();

            CreateMap<OrderItem, OrderItemDTO>()
                            .ForMember(des => des.ProductId , opt => opt.MapFrom(src => src.Product.ProductId))
                            .ForMember(des => des.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                            .ForMember(des => des.PictureUrl, opt => opt.MapFrom<OrderPictureUrlResolver>());


            CreateMap<Order, OrderToReturnDTO>()
                            .ForMember(des => des.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod.ShortName))
                            .ForMember(des => des.DeliveryCost, opt => opt.MapFrom(src => src.DeliveryMethod.Cost))
                            .ForMember(des => des.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                            .ForMember(des => des.Total, opt => opt.MapFrom(src => src.GetTotal()));

            CreateMap<DeliveryMethod, DeliveryMethodDTO>().ReverseMap();
        }
    }
}
