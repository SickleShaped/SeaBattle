using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;
using SeaBattle.Services.Bot;
using SeaBattle.Services.UserService;
using System.Drawing;

namespace SeaBattle.Services.Game
{
    public class GameService : IGameService
    {
        private readonly IUserService _userService;
        private readonly IBotService _botService;
        private readonly IMemoryCache _cache;

        public GameService(IBotService botService, IMemoryCache cache, IUserService userService)
        {
            _botService = botService;
            _userService = userService;
            _cache = cache;
        }

        /// <summary>
        /// Инициализировать игровые данные
        /// </summary>
        public void InitGameData()
        {
            var ships = GetShips();
            GameCondition condition = new GameCondition(ships);
            Table PlayerTable = new Table();
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

        /// <summary>
        /// Получить всю информацию об игре
        /// </summary>
        /// <returns></returns>
        public string GetGameData()
        {
            if(!_cache.TryGetValue("Condition", out _))
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

        /// <summary>
        /// Перезапустить игру
        /// </summary>
        /// <returns></returns>
        public string RestartGame()
        {
            InitGameData();
            string json = GetGameData();
            return json;
        }

        /// <summary>
        /// Начать игру
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void StartGame()
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
            _cache.Set("Condition", condition);;
        }

        /// <summary>
        /// Функция проверки на то, выиграла ли одна или другая сторона
        /// </summary>
        /// <param name="table"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public bool CheckVictory(Table table, GameCondition condition)
        {
            int resultScore = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (table.Cells[i, j] == TilesType.Ship && table.CellsVisibility[i, j] == TilesVisibility.Checked)
                    {
                        resultScore++;
                    }
                }
            }
            
            if (resultScore == condition.WinStore)
            {
                condition.IsGameFinished = true;
                _cache.Set("Condition", condition);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Функция делания игроком своего хода
        /// </summary>
        /// <param name="json"></param>
        /// <exception cref="Exception"></exception>
        public void MakeTurn(string json)
        {
            _cache.TryGetValue("Condition", out GameCondition condition);
            if (condition.IsGameStarted == false || condition.IsGameFinished == true) { throw new Exception("Игра не началась или уже закочилась!"); }


            PlaceShipDto Cell = JsonConvert.DeserializeObject<PlaceShipDto>(json);

            _cache.TryGetValue("PlayerTable", out Table PlayerTable);
            _cache.TryGetValue("EnemyTable", out Table EnemyTable);

            Cell.CellId -= 100;

            int y = Cell.CellId % 10;
            int x = (Cell.CellId - y) / 10;

            bool gotShip = false;

            try
            {
                gotShip = _userService.Shoot(EnemyTable, x, y);
                _cache.Set("EnemyTable", EnemyTable);
                if (gotShip) { return; }
            }
            catch (Exception ex) { throw ex; }
            if (CheckVictory(EnemyTable, condition))
            {
                condition.PlayerWin = true;
                return;
            }

            _botService.Shoot();

            if(CheckVictory(EnemyTable,condition))
            {
                condition.BotWin = true;
                return;
            }
            
            return;

        }

    }
}
