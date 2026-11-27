using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs;
using E_Commerce.Application.DTOs.Authentications;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;

        public AuthenticationService(IIdentityService identityService,
                                     ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }

        public async Task<Result<bool>> CheckEmailAsync(string email, CancellationToken ct = default)
        {
            return await _identityService.EmailExistsAsync(email, ct);
        }

        public async Task<Result<AddressDTO>> GetAddressAsync(string email, CancellationToken ct = default)
        {
            return await _identityService.GetAddressByEmailAsync(email, ct);
        }

        public async Task<Result<AddressDTO>> UpdateAddressAsync(string email, AddressDTO addressDto, CancellationToken ct = default)
        {
            return await _identityService.UpdateAddressByEmailAsync(email, addressDto, ct);
        }

        public async Task<Result<UserDTO>> GetCurrentUserAsync(string email, CancellationToken ct = default)
        {
            var user = await _identityService.FindByEmailAsync(email, ct);
            if (!user._isSuccess)
            {
                return Result<UserDTO>.Fail(user._errors);
            }

            var roles = await _identityService.GetRolesAsync(user.data.Email!, ct);
            var token = _tokenService.CreateToken(user.data.Id, user.data.Email!, user.data.UserName!, roles.data);

            return Result<UserDTO>.Ok(
               new UserDTO { Email = user.data.Email!, DisplayName = user.data.DisplayName, Token = token }
               );
        }

        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDto, CancellationToken ct = default)
        {
            var user = await _identityService.FindByEmailAsync(loginDto.Email, ct);
            if (!user._isSuccess)
            {
                return Result<UserDTO>.Fail(user._errors);
            }
            var passCheck = await _identityService.CheckPasswordAsync(loginDto.Email,loginDto.Password, ct);
            if (!passCheck._isSuccess)
            {
                return Result<UserDTO>.Fail(passCheck._errors);
            }
            var roles = await _identityService.GetRolesAsync(user.data.Email!, ct);
            var token = _tokenService.CreateToken(user.data.Id, user.data.Email!, user.data.UserName!, roles.data ) ;

            return Result<UserDTO>.Ok(
                new UserDTO { Email = user.data.Email!, DisplayName = user.data.DisplayName, Token = token }
                );
        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDto, CancellationToken ct = default)
        {
            var result = await _identityService.CreateUserAsync(registerDto, ct);
            if (!result._isSuccess)
            {
                return Result<UserDTO>.Fail(result._errors);
            }

            var roles = await _identityService.GetRolesAsync(result.data.Email!, ct);
            var token = _tokenService.CreateToken(result.data.Id, result.data.Email!, result.data.UserName!, roles.data);

            return Result<UserDTO>.Ok(
                new UserDTO { Email = result.data.Email!, DisplayName = result.data.DisplayName, Token = token }
                );
        }

    }
}
