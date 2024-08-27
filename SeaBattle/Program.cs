using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using SeaBattle.Extensions;
using SeaBattle.Services;
using SeaBattle.Services.Implementations.Consumer;
using System.Net.WebSockets;

namespace SeaBattle;

public class Program
{
    public static void Main()
    {
        var builder = WebApplication.CreateBuilder();


        builder.Services.AddControllersWithViews();
        var connection = builder.Configuration.GetConnectionString("Default");
        builder.Services.AddDependencyInjection(builder.Configuration);
        //builder.Services.AddHostedService<ConsumerHostedService>();
        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseHsts();
        }

        app.UseMiddleware<MiddlewareBuilderService>();
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        var webSocketOptions = new WebSocketOptions
        {
            KeepAliveInterval = TimeSpan.FromMinutes(2)
        };
        app.UseWebSockets(webSocketOptions);

        app.Use(async (context, next) =>
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    var login = context.Request.Headers.UserAgent.ToString();
                    SocketService.SetSocket(login, webSocket, builder.Configuration);
                    var x = SocketService.GetSocket(login);
                    await Echo(webSocket);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
            }
            else
            {
                await next(context);
            }

        });

        app.UseRouting();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }

    private static async Task Echo(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        while (!result.CloseStatus.HasValue)
        {
            await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }
        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
    }
}
