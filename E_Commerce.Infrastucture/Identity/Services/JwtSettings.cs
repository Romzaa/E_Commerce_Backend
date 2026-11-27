using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastucture.Identity.Services
{
    public class JwtSettings
    {
            public string SecretKey { get; set; } = default!;
            public string Issuer { get; set; } = default!;
            public string Audience { get; set; } = default!;
            public int ExpirationMinutes { get; set; } = 60;
        
    }
}
