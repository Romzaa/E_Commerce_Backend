using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs;
using E_Commerce.Application.DTOs.Authentications;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Contracts
{
    public interface IAuthenticationService
    {
        Task<Result<UserDTO>> LoginAsync(LoginDTO loginDto, CancellationToken ct =default); 
        Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDto, CancellationToken ct =default); 
        Task<Result<bool>> CheckEmailAsync(string email, CancellationToken ct =default);
        Task<Result<UserDTO>> GetCurrentUserAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDTO>> GetAddressAsync(string email, CancellationToken ct =default);
        Task<Result<AddressDTO>> UpdateAddressAsync(string email, AddressDTO addressDto, CancellationToken ct =default);
    }
}
