using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.DTOs
{
    public class AdminUsersDto
    {
        public string Id { get; set; } = default!;
        public string? Email { get; set; } = default!;
        public string? UserName { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string Roles { get; set; } = default!;
    }
}
