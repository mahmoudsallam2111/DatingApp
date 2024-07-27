using DatingApp.Application.Helpers;
using DatingApp.Application.Interfaces;
using DatingApp.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddScoped<IUserAppService,UserAppService>();
            services.AddScoped<ITokenService,TokenService>();
            services.AddScoped<IPhotoAppService,PhotoAppService>();
            services.AddScoped<ILikeAppService,LikeAppService>();
            services.AddScoped<IMessagesAppService,MessagesAppService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
