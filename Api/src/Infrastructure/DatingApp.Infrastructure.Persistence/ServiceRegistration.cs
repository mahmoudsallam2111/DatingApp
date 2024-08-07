﻿using DatingApp.Application.Interfaces;
using DatingApp.Application.Interfaces.Repositories;
using DatingApp.Domain.Aggregates.AppUser.Entities;
using DatingApp.Infrastructure.Persistence.Context;
using DatingApp.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DatingApp.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddPersistenceInfrastructure(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                   options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Configure identity (register it)
            services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
            })
            .AddRoles<AppRole>()
            .AddRoleManager<RoleManager<AppRole>>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            // regitser Unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.RegisterRepositories();
            return services;
        }

        private static async void RegisterRepositories(this IServiceCollection services)
        {
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            var interfaceType = typeof(IGenericRepository<>);
            var interfaces = Assembly.GetAssembly(interfaceType).GetTypes()
                .Where(p => p.GetInterface(interfaceType.Name) != null);

            var implementations = Assembly.GetAssembly(typeof(GenericRepository<>)).GetTypes();

            foreach (var item in interfaces)
            {
                var implementation = implementations.FirstOrDefault(p => p.GetInterface(item.Name) != null);
                services.AddTransient(item, implementation);
            }
        }

    }
}
