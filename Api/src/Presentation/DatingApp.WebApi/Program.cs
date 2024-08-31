using DatingApp.Application;
using DatingApp.Application.Helpers;
using DatingApp.Domain.Aggregates.AppUser.Entities;
using DatingApp.Infrastructure.Persistence;
using DatingApp.Infrastructure.Persistence.Context;
using DatingApp.Infrastructure.Persistence.Context.SeedingData;
using DatingApp.WebApi.Handlers;
using DatingApp.WebApi.Infrastracture.Extensions;
using DatingApp.WebApi.Infrastracture.Filters;
using DatingApp.WebApi.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

#region RegisterServices


builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddAnyCors();
builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

builder.Services.AddScoped<LogUserActivity>();
builder.Services.AddSignalR(s =>
{
    if (!builder.Environment.IsProduction())   // use it only in development mode
    {
        s.EnableDetailedErrors = true;   // is dangerous in term of attacking , cause it provide sensitive data on the browser
    }
});
builder.Services.AddSingleton<PresenceTracker>();   // since we not want to distory it once http request completed
#endregion


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ** seed the db when create the database
using (var scope = app.Services.CreateScope())   
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
    await context.Database.ExecuteSqlRawAsync("truncate table [Connections]");  // trancate connection table when application start
    //Seed Data
    await Seed.SeedUser(userManager , roleManager);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAnyCors();

app.UseExceptionHandler(opt => { });   // add to pipe line

app.UseAuthenticationAndAuthorization();

app.UseAuthorization();

app.MapControllers();

app.MapHub<PresenceHub>("hubs/Presence");
app.MapHub<MessageHub>("hubs/message");

app.Run();
