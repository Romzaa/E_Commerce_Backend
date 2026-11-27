using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Services
{
    public class PaymentGatewaySettings
    {
        public string SecretKey { get; set; } = default!;
        public string DefaultCurrency { get; set; } = "usd";
        public string WebhookSecret { get; set; } = default!; 
    }
}
