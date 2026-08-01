using E_Commerce.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Contracts
{
    public interface IPaymentGateway
    {
        Task<PaymentIntentResult> CreatePaymentIntent(decimal amount, string currency, CancellationToken ct = default);
        Task<PaymentIntentResult> UpdatePaymentIntent(string paymentIntentId, decimal amount,  CancellationToken ct = default);
    }
}
