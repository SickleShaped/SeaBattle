using Microsoft.AspNetCore.Http.HttpResults;
using SeaBattle.Extensions;

namespace SeaBattle;

public class Program
{
    public static void Main()
    {
        var builder = WebApplication.CreateBuilder();


        builder.Services.AddControllersWithViews();
        var connection = builder.Configuration.GetConnectionString("Default");
        builder.Services.AddDependencyInjection(builder.Configuration);
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
