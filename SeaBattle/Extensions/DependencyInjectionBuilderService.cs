﻿using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using SeaBattle.Services.Implementations;
using SeaBattle.Services.Implementations.Consumer;
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
        services.AddScoped<IProducerService, ProducerService>();
        services.AddScoped<SocketService>();

        /*services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = configuration.GetConnectionString("Default");
            opt.InstanceName = "Game";
        });*/

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Default"); ; // redis is the container name of the redis service. 6379 is the default port
            options.InstanceName = "SampleInstance";
        });

        return services;
    }
}
