using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Product;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Services
{
    internal class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<PaginatedResult<ProductDTO>>> GetAllProductsAsync(ProductQueryParams queryParams, CancellationToken ct = default)
        {
            var specs = new ProductWithTypeBrandSpecification(queryParams);
            var products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync(specs, ct);
            if (!products.Any()) 
                return Result<PaginatedResult<ProductDTO>>.Fail(new Error("Products.NotFound", "No Products Available", ErrorType.NotFound));
            var data = _mapper.Map<IReadOnlyList<ProductDTO>>(products);

            var countSpecs = new ProductCountSpecification(queryParams) ;
            var count = await _unitOfWork.GetRepository<Product, int>().CountAsync(countSpecs, ct);

            var result = new PaginatedResult<ProductDTO>(queryParams.PageSize, queryParams.PageIndex ,count , data);
            return Result<PaginatedResult<ProductDTO>>.Ok(result);

        }
        public async Task<Result<IReadOnlyList<BrandDTO>>> GetAllBrandsAsync(CancellationToken ct = default)
        {
            var brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync(ct);
            if (!brands.Any())
                return Result<IReadOnlyList<BrandDTO>>.Fail(new Error("Brands.NotFound", "No Brands Available", ErrorType.NotFound));

            var data = _mapper.Map<IReadOnlyList<BrandDTO>>(brands);
            return Result<IReadOnlyList<BrandDTO>>.Ok(data);
        }

        public async Task<Result<IReadOnlyList<TypeDTO>>> GetAllTypesAsync(CancellationToken ct = default)
        {
            var types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync(ct);
            if (!types.Any())
                return Result<IReadOnlyList<TypeDTO>>.Fail(new Error("Types.NotFound", "No Brands Available", ErrorType.NotFound));

            var data = _mapper.Map<IReadOnlyList<TypeDTO>>(types);
            return Result<IReadOnlyList<TypeDTO>>.Ok(data);
        }

        public async Task<Result<ProductDTO>> GetProductByIdAsync(int Id, CancellationToken ct = default)
        {
            var specs = new ProductWithTypeBrandSpecification(Id);
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(specs, ct);
            if(product  is null)
                return Result<ProductDTO>.Fail(new Error("Product.NotFound", "Product Is Not Found", ErrorType.NotFound));
            var data = _mapper.Map<ProductDTO>(product);
            return Result<ProductDTO>.Ok(data); 

        }

   
    }
}
