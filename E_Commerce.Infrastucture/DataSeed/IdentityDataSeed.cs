using E_Commerce.Domain.Contracts;
using E_Commerce.Infrastucture.Identity.Data;
using E_Commerce.Infrastucture.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastucture.DataSeed
{
    internal class IdentityDataSeed : IDataSeeder
    {
        private readonly StoreIdentityDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<StoreIdentityDbContext> _logger;

        public IdentityDataSeed(StoreIdentityDbContext dbContext,
                                UserManager<ApplicationUser> userManager,
                                RoleManager<IdentityRole> roleManager,
                                ILogger<StoreIdentityDbContext> logger
                                )
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            try
            {
                var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync(ct);
                if (pendingMigrations.Any())
                {
                    await _dbContext.Database.MigrateAsync(ct);
                }

                if (!await _roleManager.Roles.AnyAsync(ct))
                {
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                if (!await _userManager.Users.AnyAsync())
                {
                    var superAdmin = new ApplicationUser()
                    {
                        DisplayName = "Ahmad",
                        Email = "ahmad@mail.com",
                        PhoneNumber = "0123456789",
                        UserName = "Ahmad"
                    };

                    var result = await _userManager.CreateAsync(superAdmin, "P@ssw0rd");
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
                    }
                    else
                    {
                        _logger.LogWarning($"Couldn't Seed Default SuperAdmin User {string.Join("; ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Identity Data Seed Failed");
            }

        }


    }
}
