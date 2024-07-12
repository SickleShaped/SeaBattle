using Microsoft.AspNetCore.Cors.Infrastructure;
using SeaBattle.Services.Bot;
using SeaBattle.Services.Game;
using SeaBattle.Services.ShipService;
using SeaBattle.Services.UserService;

namespace SeaBattle.Extensions
{
    public static class AddDependencyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IBotService, BotService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IShipService, ShipService>();


            return services;
        }
    }
}
