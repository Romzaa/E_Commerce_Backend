using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Cart;
using E_Commerce.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace E_Commerce.API.Controllers
{

    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly PaymentGatewaySettings _paymentGatewaySettings;

        public PaymentsController(IPaymentService paymentService,
                                  IOptions<PaymentGatewaySettings> paymentGatewaySettings)
        {
            _paymentService = paymentService;
            _paymentGatewaySettings = paymentGatewaySettings.Value;
        }
        [HttpPost("{cartId}")]
        public async Task<ActionResult<CartDTO>> CreateOrUpdatePaymentIntent(string cartId, CancellationToken ct)
        {
            var result = await _paymentService.CreateOrUpdatePaymentIntentAsync(cartId, ct);
            return ToActionResult(result);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try 
            {
                var signature = Request.Headers["Stripe-Signature"].FirstOrDefault();

                if (string.IsNullOrWhiteSpace(signature))
                {
                    return BadRequest("Missing Stripe-Signature header.");
                }

                var stripeEvent = EventUtility.ConstructEvent(json,
                                                               signature,
                                                              _paymentGatewaySettings.WebhookSecret);
                switch (stripeEvent.Type)
                {
                    case EventTypes.PaymentIntentSucceeded:
                        var SuccededPaymen = stripeEvent.Data.Object as PaymentIntent;
                        if(SuccededPaymen is null) return BadRequest();
                        await _paymentService.PaymentSucceeded(SuccededPaymen.Id);
                        break;
                    case EventTypes.PaymentIntentPaymentFailed:
                        var FailedPayment = stripeEvent.Data.Object as PaymentIntent;
                        if (FailedPayment is null) return BadRequest();
                        await _paymentService.PaymentFailed(FailedPayment.Id);
                        break;
                }
                return Ok();
            }
            catch (StripeException ex)
            {
                return BadRequest(ex.Message);
            };
            
        }

    }
}
