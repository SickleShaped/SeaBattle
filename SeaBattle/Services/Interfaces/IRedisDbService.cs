using SeaBattle.Models.AuxilaryModels;
using StackExchange.Redis;

namespace SeaBattle.Services.Interfaces;

public interface IRedisDbService
{
    Task<Game> Get(string key);
    Task Set(string key, Game game);
    Task Delete(string login);
}
