using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Authentications;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Contracts
{
    public interface IIdentityService
    {
        Task<Result<IdentityUserResult>> FindByEmailAsync(string email, CancellationToken ct = default);
        Task<Result<bool>> CheckPasswordAsync(string email, string password, CancellationToken ct = default);
        Task<Result<IdentityUserResult>> CreateUserAsync(RegisterDTO registerDto, CancellationToken ct = default);
    }
}
