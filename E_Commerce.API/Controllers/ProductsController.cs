using E_Commerce.API.Attributes;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    public class ProductsController : ApiBaseController
    {
        private readonly IProductService _productService;

        public ProductsController( IProductService productService)
        {
            _productService = productService;
        }

        [Authorize]
        [HttpGet]
        [RedisCache(120)]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProducts([FromQuery]ProductQueryParams queryParams, CancellationToken ct)
        {
            var products = await _productService.GetAllProductsAsync(queryParams, ct);
            return ToActionResult(products);
        }
        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDTO>> GetProductById(int Id, CancellationToken ct)
        {
            var product = await _productService.GetProductByIdAsync(Id, ct);
            return ToActionResult(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<BrandDTO>>> GetAllBrands(CancellationToken ct)
        {
            var brands = await _productService.GetAllBrandsAsync(ct);
            return ToActionResult(brands);
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<TypeDTO>>> GetAllTypes(CancellationToken ct)
        {
            var types = await _productService.GetAllTypesAsync(ct);
            return ToActionResult(types);
        }
    }
}
