using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs;
using E_Commerce.Application.DTOs.Order;
using E_Commerce.Application.DTOs.Product;
using E_Commerce.Infrastucture.Attachment;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class AdminController : ApiBaseController
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IIdentityService _identityService;
        private readonly IBrandsTypesService _brandsTypesService;

        public AdminController(IProductService productService, IOrderService orderService,
            IIdentityService identityService, IBrandsTypesService brandsTypesService)
        {
            _productService = productService;
            _orderService = orderService;
            _identityService = identityService;
            _brandsTypesService = brandsTypesService;
        }

        #region Products
        [HttpGet]
        public IActionResult CheckAuthorization()
        {
            return Ok(new { isAuthorized = true });
        }

        [HttpGet("products")]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProducts([FromQuery] ProductQueryParams queryParams, CancellationToken ct)
        {
            var products = await _productService.GetAllProductsAsync(queryParams, ct);
            return ToActionResult(products);
        }

        [HttpGet("products/{Id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct([FromRoute] int Id, CancellationToken ct)
        {
            var product = await _productService.GetProductByIdAsync(Id, ct);
            return ToActionResult(product);
        }

        [HttpPut("products/{Id}")]
        public async Task<ActionResult<ProductDTO>> UpdateProduct([FromRoute] int Id, AddProductDto productDto, CancellationToken ct)
        {
            var product = await _productService.UpdateProductAsync(Id, productDto, ct);
            return ToActionResult(product);
        }

        [HttpPost("products")]
        public async Task<ActionResult<AddProductDto>> AddProduct(AddProductDto productDto, CancellationToken ct)
        {
            var product = await _productService.AddProductAsync(productDto, ct);
            return ToActionResult(product);
        }

        [HttpDelete("products/{Id}")]
        public async Task<ActionResult<bool>> DeleteProduct([FromRoute] int Id, CancellationToken ct)
        {
            var result = await _productService.DeleteProductByIdAsync(Id, ct);
            return ToActionResult(result);
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

            [HttpPost("brands")]
            public async Task<ActionResult<bool>> AddBrand(string brandName, CancellationToken ct)
            {
                var result = await _brandsTypesService.AddBrandAsync(brandName, ct);
                return ToActionResult(result);
            }

        [HttpPost("types")]
        public async Task<ActionResult<bool>> AddType(string typeName, CancellationToken ct)
        {
            var result = await _brandsTypesService.AddTypeAsync(typeName, ct);
            return ToActionResult(result);
        }
        [HttpDelete("brands")]
        public async Task<ActionResult<bool>> RemoveBrand(int brandId, CancellationToken ct)
        {
            var result = await _brandsTypesService.RemoveBrandAsync(brandId, ct);
            return ToActionResult(result);
        }
        [HttpDelete("types")]
        public async Task<ActionResult<bool>> RemoveType(int typeId, CancellationToken ct)
        {
            var result = await _brandsTypesService.RemoveTypeAsync(typeId, ct);
            return ToActionResult(result);
        }
        #endregion

        #region Orders

        [HttpGet("orders")]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> GetAllOrders(CancellationToken ct)
        {
            var result = await _orderService.GetAllOrdersAsync(GetEmailFromToken(), ct);
            return ToActionResult(result);
        }

        #endregion


        #region Users
        [HttpGet("users")]
        public async Task<ActionResult<IReadOnlyList<AdminUsersDto>>> GetAllUsers(CancellationToken ct)
        {
            var users = await _identityService.GetAllUsersAsync(ct);
            return ToActionResult(users);
        }

        [HttpGet("roles")]
        public async Task<ActionResult<IReadOnlyList<RoleDto>>> GetAllRoles(CancellationToken ct)
        {
            var roles = await _identityService.GetAllRolesAsync(ct);
            return ToActionResult(roles);
        }

        [HttpPost("roles")]
        public async Task<ActionResult<bool>> AddOrUpdateRoles(string email, string role, CancellationToken ct)
        {
            var result = await _identityService.AddOrUpdateRolesAsync(email, role, ct);
            return ToActionResult(result);
        } 
        #endregion
    }
}
