using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.DbModels;
using SeaBattle.Models.Enums;
using SeaBattle.Models.Tables;
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

            _cache.Set("Condition", condition);
            _cache.Set("PlayerTable", PlayerTable);
            _cache.Set("EnemyTable", EnemyTable);
            _cache.Set("CurrentShip", 0);


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
            _cache.Dispose();
            InitGameData();
            string json = GetGameData();
            return json;
        }










        public string GetInitialData()
        {
            var ships = GetShips();
            Table playerTable = GetTable(true);

            Table enemyTable = _botService.MakeTable(ships);


            var initGameModel = GetInitGameModel(playerTable, enemyTable, ships);
            string json = JsonConvert.SerializeObject(initGameModel);
            return json;
        }



        /// <summary>
        /// Получить InitGameModel для начала игры
        /// </summary>
        /// <param name="tables"></param>
        /// <param name="ships"></param>
        /// <returns></returns>
        public InitGameModelDto GetInitGameModel(Table playerTable, Table enemytable, List<Ship> ships)
        {
            var initGameModel = new InitGameModelDto(playerTable, enemytable, ships);
            return initGameModel;
        }



        /// <summary>
        /// Получить List таблиц для начала игры
        /// </summary>
        /// <returns></returns>
        public Table GetTable(bool belongsPlayers)
        {
            Table table = new Table(belongsPlayers);

            return table;
        }

        public string PlaceShip(string json)
        {
            PlaceShipDto dto = JsonConvert.DeserializeObject<PlaceShipDto>(json);
            int ii;
            int jj;
            GetCoordinates(dto, out ii, out jj);

            int lenght = dto.PlayerTable.Cells.GetLength(0);

            if (ii + dto.ShipLenght > lenght)
            {
                throw new Exception("Корабль за пределами массива");
            }

            for (int i = ii - 1; i < ii + dto.ShipLenght + 1; i++)
            {
                if (i >= lenght || i < 0) continue;

                for (int j = jj - 1; j <= jj + 1; j++)
                {
                    if (j >= lenght || j < 0) continue;
                    if (dto.Direction == ShipDirection.Vertical)
                    {
                        if (dto.PlayerTable.Cells[i, j] == TilesType.Ship)
                        {
                            throw new Exception("Необходимая область уже занята другим кораблем");
                        }
                    }
                    else
                    {
                        if (dto.PlayerTable.Cells[j, i] == TilesType.Ship)
                        {
                            throw new Exception("Необходимая область уже занята другим кораблем");
                        }
                    }

                }
            }
            for (int i = ii; i < ii + dto.ShipLenght; i++)
            {
                if (dto.Direction == ShipDirection.Vertical)
                {
                    dto.PlayerTable.Cells[i, jj] = TilesType.Ship;
                }
                else
                {
                    dto.PlayerTable.Cells[jj, i] = TilesType.Ship;
                }

            }

            string result = JsonConvert.SerializeObject(dto);

            return result;
        }

        private void GetCoordinates(PlaceShipDto dto, out int ii, out int jj)
        {
            int lenght = dto.PlayerTable.Cells.GetLength(0);
            int cell_x = dto.Cell % lenght;
            int cell_y = (dto.Cell - cell_x) / 10;

            if (dto.Direction == ShipDirection.Horisontal)
            {
                jj = cell_y;
                ii = cell_x;
            }
            else
            {
                jj = cell_x;
                ii = cell_y;
            }
        }

       




    }
}
