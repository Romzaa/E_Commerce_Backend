using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.Services;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastucture.Payment
{
    internal class StripePaymentGateway : IPaymentGateway
    {
        private readonly PaymentIntentService _paymentIntentService = new PaymentIntentService();
        public StripePaymentGateway(IOptions<PaymentGatewaySettings> options)
        {
            StripeConfiguration.ApiKey = options.Value.SecretKey;
        }
        public async Task<PaymentIntentResult> CreatePaymentIntent(decimal amount, string currency, CancellationToken ct = default)
        {
            var options = new PaymentIntentCreateOptions()
            {
                Amount = (long)(amount * 100),
                Currency = currency.ToLower(),
                PaymentMethodTypes = ["card"]
            };

            var intent = await _paymentIntentService.CreateAsync(options, cancellationToken: ct);

            return new PaymentIntentResult(intent.Id, intent.ClientSecret);

        }

        public async Task<PaymentIntentResult> UpdatePaymentIntent(string paymentIntentId, decimal amount, CancellationToken ct = default)
        {
            var options = new PaymentIntentUpdateOptions()
            {
                Amount = (long)(amount * 100)
            };
            var intent = await _paymentIntentService.UpdateAsync(paymentIntentId, options,cancellationToken: ct);

            return new PaymentIntentResult(intent.Id, intent.ClientSecret);

        }
    }
}
