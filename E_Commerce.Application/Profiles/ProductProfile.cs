using AutoMapper;
using E_Commerce.Application.DTOs.Product;
using E_Commerce.Domain.Entities.Products;
using E_Commerce.Infrastucture.Attachment;
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
                .ForMember(des => des.ThumbnailUrl, opt => opt.MapFrom<ProductThumbnailResolver>())
                .ForMember(des => des.Images, opt => opt.MapFrom<ProductImagesResolver>());
            CreateMap<AddProductDto, Product>()
                    .ForMember(des => des.BrandId, opt => opt.MapFrom(src => src.BrandId))
                    .ForMember(des => des.TypeId, opt => opt.MapFrom(src => src.TypeId))
                    .ForMember(des => des.ThumbnailUrl, opt => opt.Ignore())
                    .ForMember(des => des.Images, opt => opt.Ignore()).ReverseMap();
                
        }
    }
}
