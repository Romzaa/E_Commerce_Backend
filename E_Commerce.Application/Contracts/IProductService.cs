using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Product;
using E_Commerce.Infrastucture.Attachment;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Contracts
{
    public interface IProductService
    {
        Task<Result<PaginatedResult<ProductDTO>>> GetAllProductsAsync(ProductQueryParams queryParams, CancellationToken ct = default);
        Task<Result<IReadOnlyList<BrandDTO>>> GetAllBrandsAsync(CancellationToken ct = default);
        Task<Result<IReadOnlyList<TypeDTO>>> GetAllTypesAsync(CancellationToken ct = default);
        Task<Result<ProductDTO>> GetProductByIdAsync(int Id, CancellationToken ct = default);
        Task<Result<bool>> DeleteProductByIdAsync(int Id, CancellationToken ct = default);
        Task<Result<AddProductDto>> AddProductAsync(AddProductDto productDto, CancellationToken ct = default);
        Task<Result<ProductDTO>> UpdateProductAsync(int Id, AddProductDto productDto, CancellationToken ct = default);



    }
}
