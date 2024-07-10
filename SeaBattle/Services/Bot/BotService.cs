using SeaBattle.Models.Tables;
using SeaBattle.Models;
using Microsoft.Extensions.Caching.Memory;

namespace SeaBattle.Services.Bot
{
    public class BotService:IBotService
    {
        IMemoryCache cache;

        public BotService(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public Table MakeTable(List<Ship> ships)
        {
            Table table = new Table(false);

            ///тут логика заполнения кораблями таблицы бота
            foreach (var ship in ships)
            {
                while (false)
                {
                    try
                    {

                    }
                    catch (Exception ex) { }
                }
            }

            return table;
        }
    }
}
