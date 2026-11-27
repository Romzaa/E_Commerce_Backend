using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs;
using E_Commerce.Application.DTOs.Authentications;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Contracts
{
    public interface IIdentityService
    {
        Task<Result<IdentityUserResult>> FindByEmailAsync(string email, CancellationToken ct = default);
        Task<Result<IReadOnlyList<AdminUsersDto>>> GetAllUsersAsync(CancellationToken ct = default);
        Task<Result<bool>> CheckPasswordAsync(string email, string password, CancellationToken ct = default);
        Task<Result<IdentityUserResult>> CreateUserAsync(RegisterDTO registerDto, CancellationToken ct = default);
        Task<Result<IReadOnlyList<string>>> GetRolesAsync(string email, CancellationToken ct = default);
        Task<Result<bool>> AddOrUpdateRolesAsync(string email, string role, CancellationToken ct = default);
        Task<Result<bool>> EmailExistsAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDTO>> GetAddressByEmailAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDTO>> UpdateAddressByEmailAsync(string email, AddressDTO addressDTO, CancellationToken ct = default);

        Task<Result<IReadOnlyList<RoleDto>>> GetAllRolesAsync( CancellationToken ct = default);

    }
}
