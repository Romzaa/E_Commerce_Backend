using E_Commerce.Infrastucture.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastucture.Identity.Data
{
    internal class StoreIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public StoreIdentityDbContext(DbContextOptions<StoreIdentityDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Address>().ToTable("Addresses");
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");

            builder.Entity<Address>().HasOne(a => a.User)
                                     .WithOne(u => u.Address)
                                     .HasForeignKey<Address>(a => a.UserId)
                                     .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
