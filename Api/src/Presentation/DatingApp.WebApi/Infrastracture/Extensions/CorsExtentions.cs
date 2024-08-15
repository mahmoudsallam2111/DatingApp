namespace DatingApp.WebApi.Infrastracture.Extensions
{
    public static class CorsExtentions
    {
        public static IServiceCollection AddAnyCors(this IServiceCollection services)
        {
            return services.AddCors(x =>
            {
                x.AddPolicy("Any", b =>
                {
                    b.AllowAnyHeader();
                    b.AllowAnyMethod();
                    b.AllowCredentials();   // required for signalR
                    b.WithOrigins("http://localhost:4200");
                });
            });
        }
        public static IApplicationBuilder UseAnyCors(this IApplicationBuilder app)
        {
            return app.UseCors("Any");
        }
    }
}
