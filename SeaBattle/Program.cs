using Microsoft.AspNetCore.Http.HttpResults;
//using SignalRApp;
using SeaBattle.Extensions;
using SeaBattle.Services;

namespace SeaBattle;

public class Program
{
    public static void Main()
    {
        var builder = WebApplication.CreateBuilder();


        builder.Services.AddControllersWithViews();
        var connection = builder.Configuration.GetConnectionString("Default");
        builder.Services.AddDependencyInjection(builder.Configuration);
        builder.Services.AddSignalR();
        builder.Services.AddHostedService<RabbitMqListener>();
        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseHsts();
        }
        

        app.UseMiddleware<MiddlewareBuilderService>();
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.MapHub<RabbitHub>("/hubs/rabbit");


        app.UseRouting();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
