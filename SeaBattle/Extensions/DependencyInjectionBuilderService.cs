using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using SeaBattle.Services.Implementations;
using SeaBattle.Services.Interfaces;
using StackExchange.Redis;

namespace SeaBattle.Extensions;

/// <summary>
/// Класс, предсотавляющий метод добавляющий в сервисы DI
/// </summary>
public static class DependencyInjectionBuilderService
{
    /// <summary>
    /// Добавить DI в сервисы
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IGamer, BotService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITableService, TableService>();
        services.AddScoped<IRedisDbService, RedisDbService>();
        services.AddScoped<ProducerService>();

        services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = configuration.GetConnectionString("Default");
            opt.InstanceName = "Game";
        });

        return services;
    }
}
