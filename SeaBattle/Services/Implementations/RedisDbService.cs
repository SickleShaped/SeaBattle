using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;
using SeaBattle.Services.Interfaces;
using StackExchange.Redis;

namespace SeaBattle.Services.Implementations
{
    public class RedisDbService(IDistributedCache distributedCache, IGamer gamer) : IRedisDbService
    {
        private readonly IDistributedCache _distributedCache = distributedCache;
        private readonly IGamer _gamer = gamer;



        public async Task<Game> Get(string login)
        {
            var result = await _distributedCache.GetStringAsync($"Game_{login}");
            if(result == RedisValue.Null)
                return await InitGameData(login);


            return JsonConvert.DeserializeObject<Game>(result);
        }
        public async Task Set(string login, Game game)
        {
            await _distributedCache.SetStringAsync($"Game_{login}", JsonConvert.SerializeObject(game)); 
        }
        public async Task Delete(string login)
        {
            await _distributedCache.RemoveAsync($"Game_{login}");
        }



        private async Task<Game> InitGameData(string login)
        {
            var ships = new List<Ship>
            {
                new Ship(4), new Ship(3), new Ship(3), new Ship(2), new Ship(2),
                new Ship(2), new Ship(1), new Ship(1), new Ship(1), new Ship(1)
            };

            Game game = new Game()
            {
                PlayerTable = new Table(),
                EnemyTable = _gamer.AutoMakeTable(ships),
                Condition = new GameCondition(ships)
                {
                    GameState = GameState.PlacingShips,
                }
            };

            await _distributedCache.SetStringAsync($"Game_{login}", JsonConvert.SerializeObject(game));
            return game;
        }

    }


}
