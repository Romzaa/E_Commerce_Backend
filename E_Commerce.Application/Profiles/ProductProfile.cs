using AutoMapper;
using E_Commerce.Application.DTOs.Product;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Profiles
{
    internal class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductBrand, BrandDTO>();
            CreateMap<ProductType, TypeDTO>();
            CreateMap<Product, ProductDTO>()
                .ForMember(des => des.ProductBrand, opt => opt.MapFrom(src => src.ProductBrand.Name))
                .ForMember(des => des.ProductType, opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(des => des.PictureUrl, opt => opt.MapFrom<ProductPictureUrlResolver>());
        }
    }
}
