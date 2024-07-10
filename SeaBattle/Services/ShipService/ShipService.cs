using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
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
                _cache.Set("ShipDirection", ShipDirection.Vertical);
                dto.Direction = ShipDirection.Vertical;
            }
            else
            {
                _cache.Set("ShipDirection", ShipDirection.Horisontal);
                dto.Direction = ShipDirection.Horisontal;
            }


            json = JsonConvert.SerializeObject(dto);
            return json;
        }
    }
}
