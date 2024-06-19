using Microsoft.AspNetCore.Authentication.JwtBearer;
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
