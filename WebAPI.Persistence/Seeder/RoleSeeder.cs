using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Domain.Entities;

namespace WebAPI.Persistence.Seeder
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<UserRole> _roleManager)
        {
            var rolesCount = await _roleManager.Roles.CountAsync();
            if (rolesCount <= 0)
            {

                await _roleManager.CreateAsync(new UserRole()
                {
                    Name = "admin",
                   
                });
                await _roleManager.CreateAsync(new UserRole()
                {
                    Name = "user",
                    
                });
                await _roleManager.CreateAsync(new UserRole()
                {
                    Name = "auditor",
                   
                });
            }
        }
    }
}
