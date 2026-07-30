using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Authentications;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly IIdentityService _identityService;

        public AuthenticationService(IIdentityService identityService)
        {
            _identityService = identityService;
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

            return Result<UserDTO>.Ok(
                new UserDTO { Email = user.data.Email!, DisplayName = user.data.DisplayName, Token = "Token - ToDo" }
                );
        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDto, CancellationToken ct = default)
        {
            var result = await _identityService.CreateUserAsync(registerDto, ct);
            if (!result._isSuccess)
            {
                return Result<UserDTO>.Fail(result._errors);
            }
            return Result<UserDTO>.Ok(
                new UserDTO { Email = result.data.Email!, DisplayName = result.data.DisplayName, Token = "Token - ToDo" }
                );
        }
    }
}
