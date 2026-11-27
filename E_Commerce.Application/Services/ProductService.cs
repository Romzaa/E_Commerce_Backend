using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Product;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Products;
using E_Commerce.Infrastucture.Attachment;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Services
{
    internal class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachmentService = attachmentService;
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
            if (count < 1)
                return Result<PaginatedResult<ProductDTO>>.Fail(new Error("Products.NotFound", "No Products Available", ErrorType.NotFound));

            var result = new PaginatedResult<ProductDTO>(queryParams.PageSize, queryParams.PageIndex ,count , data);
            return result.Count > 0 ?  Result<PaginatedResult<ProductDTO>>.Ok(result) 
                : Result<PaginatedResult<ProductDTO>>.Fail(new Error("Products.NotFound", "No Products Available", ErrorType.NotFound));

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

        public async Task<Result<bool>> DeleteProductByIdAsync(int Id, CancellationToken ct = default)
        {
            var specs = new ProductWithTypeBrandSpecification(Id);
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(specs, ct);
            if (product is null)
                return Result<bool>.Fail(new Error("Product.NotFound", "Product Is Not Found", ErrorType.NotFound));

            _unitOfWork.GetRepository<Product, int>().Remove(product);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            if (result > 0)
            {
                _attachmentService.DeleteFile(product.ThumbnailUrl, "thumbnails/products");
                foreach (var image in product.Images)
                {
                    _attachmentService.DeleteFile(image.Url, "images/products");
                }
            }
            return result > 0? Result<bool>.Ok(true) 
                : Result<bool>.Fail(new Error("Product.DeleteFailed", "Failed To Delete Product", ErrorType.Failure));
        }

        public async Task<Result<AddProductDto>> AddProductAsync(AddProductDto productDto, CancellationToken ct = default)
        {
            var productRepo = _unitOfWork.GetRepository<Product, int>();
            var product = _mapper.Map<Product>(productDto);
            var thumbnailUrl = await _attachmentService.UploadAsync(productDto.ThumbnailUrl!.OpenReadStream(), productDto.ThumbnailUrl!.FileName,"Files/thumbnails/products",ct);
            product.ThumbnailUrl= $"thumbnails/products/{thumbnailUrl.data}";
            var newImages = new List<Image>();
            foreach (var image in productDto.Images)
            {
                var imageUrl = await _attachmentService.UploadAsync(image!.OpenReadStream(), image!.FileName, "Files/images/products", ct);
                newImages.Add(new Image { Url = $"images/products/{imageUrl.data}" });
            }
            product.Images = newImages;
            productRepo.Add(product);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            if(result < 1)
            {
                var thumbName = Path.GetFileName(product.ThumbnailUrl);
                _attachmentService.DeleteFile(thumbName, "Files/thumbnails/products");
                foreach (var image in product.Images)
                {
                    var imageName = Path.GetFileName(image.Url);
                    _attachmentService.DeleteFile(imageName, "Files/images/products");
                }   
            }
            return result > 0 ? Result<AddProductDto>.Ok(productDto) : Result<AddProductDto>.Fail(new Error("Product.AddFailed", "Failed To Add Product", ErrorType.Failure));

        }
        public async Task<Result<ProductDTO>> UpdateProductAsync(int Id, AddProductDto productDto, CancellationToken ct = default)
        {
            var specs = new ProductWithTypeBrandSpecification(Id);
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(specs, ct);
            if (product is null)
                return Result<ProductDTO>.Fail(new Error("Product.NotFound", "Product Is Not Found", ErrorType.NotFound));
            product.Name = productDto.Name ?? product.Name;
            product.BrandId = productDto.BrandId;
            product.TypeId = productDto.TypeId;
            product.Description = productDto.Description ?? product.Description;
            product.StockQuantity = productDto.StockQuantity == 0 ? product.StockQuantity : productDto.StockQuantity;
            product.Price = productDto.Price == 0 ? product.Price : productDto.Price;
            product.DiscountPercentage = productDto.DiscountPercentage== 0 ? product.DiscountPercentage : productDto.DiscountPercentage;

            if (productDto.ThumbnailUrl is not null)
            {
                var thumbName = Path.GetFileName(product.ThumbnailUrl);
                _attachmentService.DeleteFile(thumbName, "Files/thumbnails/products");

                var thumbnailUrl = await _attachmentService.UploadAsync(productDto.ThumbnailUrl!.OpenReadStream(), productDto.ThumbnailUrl!.FileName, "Files/thumbnails/products", ct);
                product.ThumbnailUrl = $"thumbnails/products/{thumbnailUrl.data}";

            }else if(productDto.Images is not null && productDto.Images.Count > 0)
            { 
                foreach (var image in product.Images)
                {
                    var imageName = Path.GetFileName(image.Url);
                    _attachmentService.DeleteFile(imageName, "Files/images/products");
                }
                foreach (var image in productDto.Images)
                {
                    var imageUrl = await _attachmentService.UploadAsync(image!.OpenReadStream(), image!.FileName, "Files/images/products", ct);
                    product.Images.Add(new Image { Url = $"images/products/{imageUrl.data}" });
                }
            }
            _unitOfWork.GetRepository<Product, int>().Update(product);
                    var resultf = await _unitOfWork.SaveChangesAsync(ct);
                    return resultf > 0 ? Result<ProductDTO>.Ok(_mapper.Map<ProductDTO>(product))
                        : Result<ProductDTO>.Fail(new Error("Product.UpdateFailed", "Failed To Update Product", ErrorType.Failure));
        }
    }
}
