using SeaBattle.Models;
using Microsoft.Extensions.Caching.Memory;
using SeaBattle.Models.Enums;
using Newtonsoft.Json;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.DbModels;

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
                while (true)
                {
                    try
                    {
                        BotPlaceShip(ship, table);
                        break;
                    }
                    catch (Exception ex) { continue; }
                }
            }

            return table;
        }

        private void BotPlaceShip(Ship ship, Table table) //не знаю, это нарушение DRY или нет?
        {
            int size = 10;
            Random rand = new Random();
            int xx = rand.Next(0, size);
            int yy = rand.Next(0, size);
            ShipDirection direction = (ShipDirection)rand.Next(0, 2);

            int ii; int jj;
            if (direction == ShipDirection.Horisontal)
            {
                jj = yy;
                ii = xx;
            }
            else
            {
                jj = xx;
                ii = yy;
            }
            if (ii + ship.Lenght > size)
            {
                throw new Exception();
            }
            for (int i = ii - 1; i < ii + ship.Lenght + 1; i++)
            {
                if (i >= size || i < 0) continue;

                for (int j = jj - 1; j <= jj + 1; j++)
                {
                    if (j >= size || j < 0) continue;
                    if (direction == ShipDirection.Vertical)
                    {
                        if (table.Cells[i, j] == TilesType.Ship)
                        {
                            throw new Exception();
                        }
                    }
                    else
                    {
                        if (table.Cells[j, i] == TilesType.Ship)
                        {
                            throw new Exception();
                        }
                    }

                }
            }
            for (int i = ii; i < ii + ship.Lenght; i++)
            {
                if (direction == ShipDirection.Vertical)
                {
                    table.Cells[i, jj] = TilesType.Ship;
                }
                else
                {
                    table.Cells[jj, i] = TilesType.Ship;
                }

            }


        }

    }
}
