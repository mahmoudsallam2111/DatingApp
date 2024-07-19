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
    public static async Task SeedUser(UserManager<AppUser> userManager)
    {
        if(! await userManager.Users.AnyAsync())
        {
            var userData = File.ReadAllText("D:\\Mahmoud\\DatingApp\\Api\\src\\Infrastructure\\DatingApp.Infrastructure.Persistence\\Context\\SeedingData\\UserSeedData.json");  // read from json file
            var options  = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);
            if (users is null)  return;

            foreach (var user in users)
            {
               user.UserName = user?.UserName?.ToLower();
                var result = await userManager.CreateAsync(user, "Mah1234");
                if (!result.Succeeded)
                {
                    // Log the errors
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine(error.Description);
                    }
                }
            }
        }
    }
}
