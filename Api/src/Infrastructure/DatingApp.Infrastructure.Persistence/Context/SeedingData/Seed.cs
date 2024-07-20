using CloudinaryDotNet.Actions;
using DatingApp.Application.Dtos;
using DatingApp.Application.Interfaces;
using DatingApp.Domain.Aggregates.AppUser.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace DatingApp.Infrastructure.Persistence.Context.SeedingData;

public static class Seed
{
    public static async Task SeedUser(UserManager<AppUser> userManager , RoleManager<AppRole> roleManager)
    {
        if(! await userManager.Users.AnyAsync())
        {
            var userData = File.ReadAllText("D:\\Mahmoud\\DatingApp\\Api\\src\\Infrastructure\\DatingApp.Infrastructure.Persistence\\Context\\SeedingData\\UserSeedData.json");  // read from json file
            var options  = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);
            if (users is null)  return;

            var roles = new List<AppRole>()
            {
                new AppRole(){ Name = "Member"},
                new AppRole(){ Name = "Admin"},
                new AppRole(){ Name = "Moderator"},
            };
            foreach (var role in roles)
            {
               await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
               user.UserName = user?.UserName?.ToLower();
                var result = await userManager.CreateAsync(user, "Mah1234");
                await userManager.AddToRoleAsync(user, "Member");
                if (!result.Succeeded)
                {
                    // Log the errors
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine(error.Description);
                    }
                }
            }

            // create admin user

            var admin =  new AppUser() { UserName = "admin" };

            await userManager.CreateAsync(admin, "Mah1234");
            await userManager.AddToRolesAsync(admin, new[] { "Admin" , "Moderator" });

        }
    }
}
