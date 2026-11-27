using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastucture.Identity.Entities
{
    internal class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; } = default!;
        public Address? Address { get; set; }
    }
}
