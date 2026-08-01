using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Common
{
    public class PaymentIntentResult
    {
        public PaymentIntentResult(string paymentIntentId, string clientSecret)
        {
            PaymentIntentId = paymentIntentId;
            ClientSecret = clientSecret;
        }

        public string PaymentIntentId { get; } = default!;
        public string ClientSecret { get; } = default!;
    }
}
