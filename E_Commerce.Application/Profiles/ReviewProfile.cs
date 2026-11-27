using AutoMapper;
using E_Commerce.Application.DTOs.Product;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Profiles
{
    internal class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewDTO>().ReverseMap();
        }

    }
}
