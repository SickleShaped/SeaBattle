using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.DbModels;
using SeaBattle.Models.Enums;
using SeaBattle.Services.Bot;
using System.Drawing;

namespace SeaBattle.Services.Game
{
    public class GameService : IGameService
    {
        private readonly IBotService _botService;
        private readonly IMemoryCache _cache;

        public GameService(IBotService botService, IMemoryCache cache)
        {
            _botService = botService;
            _cache = cache;
        }

        public void InitGameData()
        {
            var ships = GetShips();
            GameCondition condition = new GameCondition(ships);
            Table PlayerTable = new Table(true);
            Table EnemyTable = _botService.MakeTable(ships);

            _cache.Set("Condition", condition, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
            _cache.Set("PlayerTable", PlayerTable, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
            _cache.Set("EnemyTable", EnemyTable, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
            _cache.Set("CurrentShip", 0, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));


        }

        /// <summary>
        /// Получить List кораблей для начала игры
        /// </summary>
        /// <returns></returns>
        public List<Ship> GetShips()
        {
            var ships = new List<Ship>();
            ships.Add(new Ship(4));
            ships.Add(new Ship(3));
            ships.Add(new Ship(3));
            ships.Add(new Ship(2));
            ships.Add(new Ship(2));
            ships.Add(new Ship(2));
            ships.Add(new Ship(1));
            ships.Add(new Ship(1));
            ships.Add(new Ship(1));
            ships.Add(new Ship(1));
            return ships;
        }

        public string GetGameData()
        {
            _cache.TryGetValue("Condition", out GameCondition conditionCheck);
            if (conditionCheck==null)
            {
                InitGameData();
            }
            _cache.TryGetValue("PlayerTable", out Table PlayerTable);
            _cache.TryGetValue("EnemyTable", out Table EnemyTable);
            _cache.TryGetValue("Condition", out GameCondition condition);

            FullGameDto FullGameDto = new FullGameDto(PlayerTable, EnemyTable, condition);
            string json = JsonConvert.SerializeObject(FullGameDto);
            return json;


        }

        public string RestartGame()
        {
            InitGameData();
            string json = GetGameData();
            return json;
        }

        public string StartGame()
        {
            _cache.TryGetValue("Condition", out GameCondition condition);
           


            if (condition.IsGameStarted == true) { throw new Exception("Игра уже началась"); }

            _cache.TryGetValue("PlayerTable", out Table PlayerTable);

            int shipsForPoints = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (PlayerTable.Cells[i, j] == TilesType.Ship)
                    {
                        shipsForPoints++;
                    }
                }
            }

            if (shipsForPoints != condition.WinStore) { throw new Exception("Еще не все корабли расставлены!"); }


            condition.IsGameStarted = true;
            _cache.Set("Condition", condition);
            return "true";
        }

    }
}
