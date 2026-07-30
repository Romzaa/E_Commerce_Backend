using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Authentications;
using E_Commerce.Infrastucture.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastucture.Identity.Services
{
    internal class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result<bool>> CheckPasswordAsync(string email, string password, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return Result<bool>.Fail(Error.Unauthorized("User.Unauthorized", "Invalid Email Or Password"));

            var result = await _userManager.CheckPasswordAsync(user, password);
            return result ? Result<bool>.Ok(true) : Result<bool>.Fail(Error.Unauthorized("User.Unauthorized", "Invalid Email Or Password"));

        }

        public async Task<Result<IdentityUserResult>> CreateUserAsync(RegisterDTO registerDto, CancellationToken ct = default)
        {
            var user = new ApplicationUser()
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber,
                DisplayName = registerDto.DisplayName

            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => new Error(e.Code, e.Description)).ToList();
                return Result<IdentityUserResult>.Fail(errors);
            }
            return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id, user.Email, user.UserName, user.DisplayName));
        }

        public async Task<Result<IdentityUserResult>> FindByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Result<IdentityUserResult>.Fail(Error.Failure("User.NotFound", "User Is Not Found"));

            return Result<IdentityUserResult>.Ok( new IdentityUserResult(user.Id, user.Email, user.UserName, user.DisplayName));
        }
    }
}
