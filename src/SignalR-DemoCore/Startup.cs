using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SignalRDemoCore.Hubs;

namespace SignalRDemoCore
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
             {
                 builder
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                     .AllowCredentials()
                     .SetIsOriginAllowed(host => true);
             }));
            services.AddSingleton<NotificationTicker>();
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors("CorsPolicy");
            app.UseSignalR(routes =>
            {
                routes.MapHub<NotificationHub>("/notificationHub");
            });
        }
    }
}
