using Microsoft.AspNetCore.Http.HttpResults;
//using SignalRApp;
using SeaBattle.Extensions;
using SeaBattle.Services;
using SeaBattle.Services.Implementations;

namespace SeaBattle;

public class Program
{
    public static void Main()
    {
        var builder = WebApplication.CreateBuilder();


        builder.Services.AddControllersWithViews();
        var connection = builder.Configuration.GetConnectionString("Default");
        builder.Services.AddDependencyInjection(builder.Configuration);
        //builder.Services.AddHostedService<ConsumerService>();
        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseHsts();
        }
        

        app.UseMiddleware<MiddlewareBuilderService>();
        app.UseHttpsRedirection();
        app.UseStaticFiles();


        app.UseRouting();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
