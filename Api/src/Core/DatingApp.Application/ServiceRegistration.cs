using DatingApp.Application.Features.Users;
using DatingApp.Application.Interfaces;
using DatingApp.Application.Interfaces.Users;
using DatingApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddScoped<IUserAppService,UserAppService>();
            services.AddScoped<ITokenService,TokenService>();
            return services;
        }
    }
}
