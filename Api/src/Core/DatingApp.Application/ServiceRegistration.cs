using DatingApp.Application.Features.Users;
using DatingApp.Application.Interfaces.Users;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddScoped<IUserAppService,UserAppService>();
            return services;
        }
    }
}
