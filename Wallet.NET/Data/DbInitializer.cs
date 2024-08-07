using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wallet.NET.Models;

namespace Wallet.NET.Data
{
    public class DbInitializer
    {
        private readonly ModelBuilder _modelBuilder;
        public DbInitializer(ModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
        }

        internal void seed()
        {
            _modelBuilder.Entity<IdentityRole>().HasData
            (
                new IdentityRole
                {
                    Id = "d61420cf-0b6e-409e-bf51-be51a7285626",
                    Name = "User",
                    NormalizedName = "USER"
                }
            );

            var hasher = new PasswordHasher<IdentityUser>();

            _modelBuilder.Entity<User>().HasData
            (
                new User
                {
                    Id = "7c1656d0-c5fa-499a-9c41-bdc2bdefc198",
                    Email = "vitornascimento321@hotmail.com",
                    EmailConfirmed = true,
                    UserName = "vitornascimento321@hotmail.com",
                    NormalizedEmail = "VITORNASCIMENTO321@HOTMAIL.COM",
                    NormalizedUserName = "VITORNASCIMENTO321@HOTMAIL.COM",
                    PasswordHash = hasher.HashPassword(null!, "teste@123")
                }
            );

            _modelBuilder.Entity<IdentityUserRole<string>>().HasData
            (
                new IdentityUserRole<string>
                {
                    RoleId = "d61420cf-0b6e-409e-bf51-be51a7285626",
                    UserId = "7c1656d0-c5fa-499a-9c41-bdc2bdefc198"
                }
            );

        }
    }
}