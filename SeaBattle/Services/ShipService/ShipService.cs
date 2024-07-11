using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.DbModels;
using SeaBattle.Models.Enums;
using SeaBattle.Services.Bot;

namespace SeaBattle.Services.ShipService
{
    public class ShipService : IShipService
    {
        private readonly IMemoryCache _cache;

        public ShipService(IBotService botService, IMemoryCache cache)
        {
            _cache = cache;
        }

        public string FlipShip()
        {
            string json = "";
            CurrentShipDto dto = new CurrentShipDto();
            _cache.TryGetValue("CurrentShip", out int shipId);
            _cache.TryGetValue("ShipDirection", out ShipDirection shipDirection);
            _cache.TryGetValue("Condition", out GameCondition condition);
            dto.ShipLenght = condition.Ships[shipId].Lenght;

            if (shipDirection == ShipDirection.Horisontal)
            {
                _cache.Set("ShipDirection", ShipDirection.Vertical, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                dto.Direction = ShipDirection.Vertical;
            }
            else
            {
                _cache.Set("ShipDirection", ShipDirection.Horisontal, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                dto.Direction = ShipDirection.Horisontal;
            }


            json = JsonConvert.SerializeObject(dto);
            return json;
        }

        public void PlaceShip(string json)
        {
            PlaceShipDto Cell = JsonConvert.DeserializeObject<PlaceShipDto>(json);

            _cache.TryGetValue("CurrentShip", out int shipId);
            _cache.TryGetValue("ShipDirection", out ShipDirection shipDirection);
            _cache.TryGetValue("Condition", out GameCondition condition);
            _cache.TryGetValue("PlayerTable", out Table PlayerTable);

            if(condition.IsGameStarted == true) { throw new Exception("Игра уже началась"); }

            //PlaceShipDto dto = JsonConvert.DeserializeObject<PlaceShipDto>(json);
            int ii;
            int jj;
            GetCoordinates(Cell.CellId, shipDirection, out ii, out jj);

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

        private void GetCoordinates(int cellId, ShipDirection direction,  out int ii, out int jj)
        {
            int lenght = 10;
            int cell_x = cellId % lenght;
            int cell_y = (cellId - cell_x) / 10;

            if (direction == ShipDirection.Horisontal)
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

        public string GetCurrentShipLenght()
        {
            CurrentShipLenghtDto dto = new CurrentShipLenghtDto();
            _cache.TryGetValue("CurrentShip", out int shipId);
            _cache.TryGetValue("Condition", out GameCondition condition);
            try
            {
                dto.Lenght = condition.Ships[shipId].Lenght;
            }
            catch (Exception ex) { dto.Lenght = 0; }
            

            string json = JsonConvert.SerializeObject(dto);

            return json;
        }

    }
}
