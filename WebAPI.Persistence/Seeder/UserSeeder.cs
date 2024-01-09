using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Domain.Entities;

namespace WebAPI.Persistence.Seeder
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(UserManager<User> _userManager)
        {
            var usersCount = await _userManager.Users.CountAsync();
            if (usersCount <= 0)
            {
                var defaultuser = new User()
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    FirstName = "Admin First",
                    LastName = "Admin Last",
                };
                await _userManager.CreateAsync(defaultuser, "A123_a");
                await _userManager.AddToRoleAsync(defaultuser, "admin");
            }
        }
    }
}
