using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.DTOs.Authentications
{
    public class UserDTO
    {
        public string Email { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string Token { get; set; } = default!;
    }
}
