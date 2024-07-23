﻿using Microsoft.AspNetCore.Cors.Infrastructure;
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
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, Microsoft.Extensions.Configuration.ConfigurationManager builder )
    {
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<BotService>();
        services.AddScoped<UserService>();
        services.AddSingleton<ITableService, TableService>();
        services.AddScoped<IDatabase>(cfg =>
        {
            IConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect($"{builder.GetConnectionString("Default")}");
            return multiplexer.GetDatabase();
        });

        return services;
    }
}
