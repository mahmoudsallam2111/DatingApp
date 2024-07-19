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
            //// configure identity (register it)
            //services.AddIdentityCore<AppUserRole>(opt =>
            //{
            //    opt.Password.RequireNonAlphanumeric = false;
            //})
            //.AddRoles<AppRole>()
            //.AddRoleManager<RoleManager<AppRole>>()
            //.AddEntityFrameworkStores<ApplicationDbContext>();


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
               });

            services.AddAuthorization();

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
