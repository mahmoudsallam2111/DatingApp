using DatingApp.Application;
using DatingApp.Infrastructure.Persistence;
using DatingApp.WebApi.Infrastracture.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

#region RegisterServices


builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddAnyCors();
builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);


#endregion


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAnyCors();

app.UseAuthenticationAndAuthorization();

app.UseAuthorization();

app.MapControllers();

app.Run();
