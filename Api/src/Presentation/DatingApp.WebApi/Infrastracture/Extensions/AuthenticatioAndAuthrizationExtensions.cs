using DatingApp.Domain.Aggregates.AppUser.Entities;
using DatingApp.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.WebApi.Infrastracture.Extensions
{
    public static class AuthenticatioAndAuthrizationExtensions
    {
        public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services , IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config["TokenKey"]!)),
                       ValidateIssuer = false,
                       ValidateAudience = false
                   };

                   options.Events = new JwtBearerEvents
                   {
                       OnMessageReceived = context =>
                       {
                           var accessToken = context.Request.Query["access_token"];
                           var path = context.Request.Path;
                           if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                           {
                               context.Token = accessToken;
                           }
                           return Task.CompletedTask;
                       }
                   };
               });

            

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("RequiredAdminRole", policy => policy.RequireRole("Admin"));
                opt.AddPolicy("ModeratorOrAdminRole", policy => policy.RequireRole(new[] { "Admin"  , "Moderator" } ));
            });

            return services;

        }


        public static IApplicationBuilder UseAuthenticationAndAuthorization(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
    }
}
