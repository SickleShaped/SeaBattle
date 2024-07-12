using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;
using SeaBattle.Services.Bot;

namespace SeaBattle.Services.ShipService
{
    public class ShipService : IShipService
    {
        private readonly IMemoryCache _cache;

        public ShipService( IMemoryCache cache)
        {
            _cache = cache;
        }

        public string FlipShip()
        {
            CurrentShipDto dto = new CurrentShipDto();
            _cache.TryGetValue("ShipDirection", out ShipDirection shipDirection);
            _cache.TryGetValue("Condition", out GameCondition condition);

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

            var json = JsonConvert.SerializeObject(dto);
            return json;
        }

        
        public void GetFlipShipCoordinates(int cellId, ShipDirection direction,  out int ii, out int jj)
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
