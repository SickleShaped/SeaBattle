using Microsoft.AspNetCore.Cors.Infrastructure;
using SeaBattle.Services;

namespace SeaBattle.Extensions
{
    public static class AddDependencyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddTransient<IGameService, GameService>();

            return services;
        }
    }
}
