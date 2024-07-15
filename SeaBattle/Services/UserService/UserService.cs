using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;
using SeaBattle.Services.Bot;
using SeaBattle.Services.Game;
using SeaBattle.Services.ShipService;

namespace SeaBattle.Services.UserService
{
    public class UserService:IUserService
    {
        private readonly IMemoryCache _cache;
        private readonly IShipService _shipService;
        private readonly IBotService _botService;

        public UserService(IMemoryCache cache, IShipService shipService, IBotService botService)
        {
            _cache = cache;
            _shipService = shipService;
            _botService = botService;
        }

        /// <summary>
        /// Функция размещения игроком корабля
        /// </summary>
        /// <param name="json"></param>
        /// <exception cref="Exception"></exception>
        public void PlaceShip(string json)
        {
            PlaceShipDto Cell = JsonConvert.DeserializeObject<PlaceShipDto>(json);

            _cache.TryGetValue("CurrentShip", out int shipId);
            _cache.TryGetValue("ShipDirection", out ShipDirection shipDirection);
            _cache.TryGetValue("Condition", out GameCondition condition);
            _cache.TryGetValue("PlayerTable", out Table PlayerTable);

            if (condition.IsGameStarted == true) { throw new Exception("Игра уже началась"); }
            int ii;
            int jj;
            _shipService.GetFlipShipCoordinates(Cell.CellId, shipDirection, out ii, out jj);

            int lenght = 10;



            if (ii + condition.Ships[shipId].Lenght > lenght)
            {
                throw new Exception("Корабль за пределами массива");
            }

            for (int i = ii - 1; i < ii + condition.Ships[shipId].Lenght + 1; i++)
            {
                if (i >= lenght || i < 0) continue;

                for (int j = jj - 1; j <= jj + 1; j++)
                {
                    if (j >= lenght || j < 0) continue;
                    if (shipDirection == ShipDirection.Vertical)
                    {
                        if (PlayerTable.Cells[i, j] == TilesType.Ship)
                        {
                            throw new Exception("Необходимая область уже занята другим кораблем");
                        }
                    }
                    else
                    {
                        if (PlayerTable.Cells[j, i] == TilesType.Ship)
                        {
                            throw new Exception("Необходимая область уже занята другим кораблем");
                        }
                    }

                }
            }
            for (int i = ii; i < ii + condition.Ships[shipId].Lenght; i++)
            {
                if (shipDirection == ShipDirection.Vertical)
                {
                    PlayerTable.Cells[i, jj] = TilesType.Ship;
                }
                else
                {
                    PlayerTable.Cells[jj, i] = TilesType.Ship;
                }

            }
            _cache.Set("Playertable", PlayerTable, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
            _cache.Set("CurrentShip", shipId + 1, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));

        }

        /// <summary>
        /// Выстрелить в клетку. Возвращает true, если был задет корабль.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool Shoot(Table table, int x, int y)
        {
            if (table.CellsVisibility[x, y] == TilesVisibility.Checked) throw new Exception("Эта клетка уже прострелена");
            table.CellsVisibility[x, y] = TilesVisibility.Checked;
            if (table.Cells[x, y] == TilesType.Ship) { return true; }
            else { return  false; }
        }
    }
}
