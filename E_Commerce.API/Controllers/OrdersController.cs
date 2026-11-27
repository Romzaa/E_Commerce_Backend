using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController( IOrderService orderService)
        {
            _orderService = orderService;
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDTO, CancellationToken ct)
        {
            var result= await _orderService.CreateOrderAsync(orderDTO, GetEmailFromToken(), ct);
            return ToActionResult(result);
        }

        [HttpGet("deliverymethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethodDTO>>> GetDeliveryMethods(CancellationToken ct)
        {
            var result = await _orderService.GetAllDeliveryMethodsAsync(ct);
            return ToActionResult(result);
        }

        [Authorize(Roles ="SuperAdmin")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> GetAllOrders(CancellationToken ct)
        {
            var result = await _orderService.GetAllOrdersAsync(GetEmailFromToken(), ct);
            return ToActionResult(result);
        }


        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(OrderToReturnDTO) ,StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrderByIdAndEMail(Guid id, CancellationToken ct)
        {
            var result = await _orderService.GetOrderByIdAndEMailAsync(id, GetEmailFromToken(), ct);
            return ToActionResult(result);
        }

    }
}
