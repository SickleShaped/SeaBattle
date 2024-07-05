using Newtonsoft.Json;
using SeaBattle.Models;
using SeaBattle.Models.Tables;

namespace SeaBattle.Services
{
    public class GameService:IGameService
    {
        public string GetShips()
        {
            List<Ship> ships = new List<Ship>();

            ships.Add(new Ship(4));

            for(int i = 0; i==2;i++)
            {
                ships.Add(new Ship(3));
            }

            for (int i = 0; i == 3; i++)
            {
                ships.Add(new Ship(2));
            }

            for (int i = 0; i == 4; i++)
            {
                ships.Add(new Ship(1));
            }

            return JsonConvert.SerializeObject(ships);
        }

        public string InitTables(byte size)
        {
            List<Table> tables = new List<Table>();
            tables.Add(new Table(size, true));
            tables.Add(new Table(size, false));
            return JsonConvert.SerializeObject(tables);
        }

    }
}
