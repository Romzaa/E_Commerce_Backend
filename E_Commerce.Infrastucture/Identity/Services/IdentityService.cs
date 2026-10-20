using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs;
using E_Commerce.Application.DTOs.Authentications;
using E_Commerce.Infrastucture.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastucture.Identity.Services
{
    internal class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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

        public async Task<Result<bool>> EmailExistsAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return Result<bool>.Fail(Error.Unauthorized("User.NotFound", $"User:{email} Is Not Found"));
            return Result<bool>.Ok(true);
        }

        public async Task<Result<IdentityUserResult>> FindByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Result<IdentityUserResult>.Fail(Error.Failure("User.NotFound", "User Is Not Found"));

            return Result<IdentityUserResult>.Ok( new IdentityUserResult(user.Id, user.Email, user.UserName, user.DisplayName));
        }

        public async Task<Result<AddressDTO>> GetAddressByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email, ct);
            if (user is null)
                return Result<AddressDTO>.Fail(Error.Failure("User.NotFound", "User Is Not Found"));
            if(user.Address is null)
                return Result<AddressDTO>.Fail(Error.Failure("UserAddress.NotFound", "Address Is Not Found"));

            return Result<AddressDTO>.Ok(new AddressDTO()
            {
                FirstName = user.Address.FirstName,
                LastName= user.Address.LastName,
                Country = user.Address.Country,
                Street = user.Address.Street,
                City= user.Address.City
            });

        }

        public async Task<Result<AddressDTO>> UpdateAddressByEmailAsync(string email, AddressDTO addressDTO, CancellationToken ct = default)
        {
            var user = await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email, ct);
            if (user is null)
                return Result<AddressDTO>.Fail(Error.Failure("User.NotFound", "User Is Not Found"));
           
            var newAddress = new Address()
            {
                FirstName = addressDTO.FirstName,
                LastName = addressDTO.LastName,
                Country = addressDTO.Country,
                Street = addressDTO.Street,
                City = addressDTO.City
            };
            if (user.Address is null) 
            { 
                user.Address = newAddress;
            }
            else
            {
                user.Address.FirstName = addressDTO.FirstName;
                user.Address.LastName = addressDTO.LastName;
                user.Address.Country = addressDTO.Country;
                user.Address.Street = addressDTO.Street;
                user.Address.City = addressDTO.City;
            }

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded ? Result<AddressDTO>.Ok(addressDTO)
                                    : Result<AddressDTO>.Fail(Error.Failure("UserAddress.Failure", string.Join("; ", result.Errors.Select(e => e.Description).ToList())));
        }
        public async Task<Result<IReadOnlyList<string>>> GetRolesAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Result<IReadOnlyList<string>>.Fail(Error.Failure("User.NotFound", "User Is Not Found"));

            var roles = await _userManager.GetRolesAsync(user);

            return Result<IReadOnlyList<string>>.Ok(roles.ToList());
        }

        public async Task<Result<IReadOnlyList<AdminUsersDto>>> GetAllUsersAsync(CancellationToken ct = default)
        {
            var users = _userManager.Users.Select(u => new IdentityUserResult(u.Id, u.Email, u.UserName, u.DisplayName)).ToList();

            var adminUsers = new List<AdminUsersDto>();
            foreach (var user in users)
            {
                var u = await _userManager.FindByEmailAsync(user.Email!);
                var roles = await _userManager.GetRolesAsync(u!);
                var newUser = new AdminUsersDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    DisplayName = user.DisplayName,
                    Roles = roles != null ? string.Join(", ", roles) : string.Empty
                };
                adminUsers.Add(newUser);
            }

            return Result<IReadOnlyList<AdminUsersDto>>.Ok(adminUsers);
        }


        public async Task<Result<bool>> AddOrUpdateRolesAsync(string email, string role, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            
            if (user is null)
                return Result<bool>.Fail(Error.Failure("User.NotFound", "User Is Not Found"));

            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Count > 0)
            
                foreach (var r in userRoles)
                {
                    await _userManager.RemoveFromRoleAsync(user, r);
                }

                var result = await _userManager.AddToRoleAsync(user, role);

                return result.Succeeded ? Result<bool>.Ok(true)
                    : Result<bool>.Fail(Error.Failure("UserRole.Failure", string.Join("; ", result.Errors.Select(e => e.Description).ToList())));
            
        }

        public Task<Result<IReadOnlyList<RoleDto>>> GetAllRolesAsync(CancellationToken ct = default)
        {
            var roles = _roleManager.Roles.Select(r => new RoleDto
                                        {
                                            Id = r.Id,
                                            Name = r.Name!
                                        })
                                        .ToList();

            return Task.FromResult(Result<IReadOnlyList<RoleDto>>.Ok(roles));
        }
    }
}
