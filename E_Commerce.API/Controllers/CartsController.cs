using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Cart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    public class CartsController : ApiBaseController
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CartDTO>> GetCart(string id, CancellationToken ct)
        {
            var cart = await _cartService.GetCartAsync(id, ct);
            return ToActionResult<CartDTO>(cart);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartDTO>> CreateOrUpdateCart(CartDTO cart, CancellationToken ct)
        {
            var result = await _cartService.CreateOrUpdateCartAsync(cart, ct: ct);
            return ToActionResult(result);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteCart(string id, CancellationToken ct)
        {
            var result = await _cartService.DeleteCartAsync(id, ct);
            return ToActionResult(result);
        }

    }
}
