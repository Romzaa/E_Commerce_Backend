using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Contracts
{
    public interface ITokenService
    {
        string CreateToken(string userId, string email, string userName, IEnumerable<string> roles);
    }
}
