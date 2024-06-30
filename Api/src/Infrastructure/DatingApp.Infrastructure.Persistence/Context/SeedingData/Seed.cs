using DatingApp.Application.Dtos;
using DatingApp.Application.Interfaces;
using DatingApp.Domain.Aggregates.AppUser.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace DatingApp.Infrastructure.Persistence.Context.SeedingData;

public static class Seed
{
    public static async Task SeedUser(ApplicationDbContext context)
    {
        if(! await context.Users.AnyAsync())
        {
            var userData = File.ReadAllText("D:\\Mahmoud\\DatingApp\\Api\\src\\Infrastructure\\DatingApp.Infrastructure.Persistence\\Context\\SeedingData\\UserSeedData.json");  // read from json file
            var options  = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);
            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();
               user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("123456"));
                user.PasswordSalt = hmac.Key;
            }

           await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }
    }
}
