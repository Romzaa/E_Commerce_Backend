using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Cart;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Contracts
{
    public interface IPaymentService
    {
        Task<Result<CartDTO>> CreateOrUpdatePaymentIntentAsync(string cartId, CancellationToken ct = default);
        Task PaymentSucceeded(string paymentIntentId);
        Task PaymentFailed(string paymentIntentId);
    }
}
