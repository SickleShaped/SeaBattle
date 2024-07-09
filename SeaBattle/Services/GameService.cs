using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;
using SeaBattle.Models.Tables;
using System.Drawing;

namespace SeaBattle.Services
{
    public class GameService:IGameService
    {

        public string GetInitialData()
        {
            byte tableSize = 10;
            Table playerTable = GetTable(tableSize, true);
            Table enemyTable = GetTable(tableSize, false);
            var ships = GetShips();

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
        /// Получить List таблиц для начала игры
        /// </summary>
        /// <returns></returns>
        public Table GetTable(byte size, bool belongsPlayers)
        {
            Table table = new Table(size, belongsPlayers);

            return table;
        }

        public string PlaceShip(string json)
        {
            PlaceShipDto dto = JsonConvert.DeserializeObject<PlaceShipDto>(json);

            switch(dto.Direction)
            {
                case ShipDirection.Horisontal:
                    PlaceHorisontalShip(dto);
                    
                    break;

                case ShipDirection.Vertical:
                    PlaceVerticalShip(dto);
                    break;
            }

            string result = JsonConvert.SerializeObject(dto);

            return result;
        }

        public PlaceShipDto PlaceVerticalShip(PlaceShipDto dto)
        {
            int lenght = dto.PlayerTable.Cells.GetLength(0);
            int j = dto.Cell % lenght;
            int i = (dto.Cell - j) / 10;


            if (i + dto.ShipLenght > lenght)
            {
                throw new Exception("Корабль за пределами массива");
            }
            for (int ii = i - 1; ii <= i + dto.ShipLenght + 1; ii++)
            {
                if (ii < lenght) continue;
                for (int jj = j - 1; jj <= j  + 1; jj++)
                {
                    if (jj < lenght) continue;
                    if (dto.PlayerTable.Cells[ii, jj] == TilesType.Ship)
                    {
                        throw new Exception("Необходимая область уже занята другим кораблем");
                    }

                }
            }
            for (int ii = i; ii < i + dto.ShipLenght; ii++)
            {
                dto.PlayerTable.Cells[ii, j] = TilesType.Ship;
            }

            return dto;
        }

        public PlaceShipDto PlaceHorisontalShip(PlaceShipDto dto)
        {
            int lenght = dto.PlayerTable.Cells.GetLength(0);
            int j = dto.Cell % lenght;
            int i = (dto.Cell - j) / 10;
            

            if (j+dto.ShipLenght > lenght)
            {
                throw new Exception("Корабль за пределами массива");
            }
            for (int ii = i - 1; ii <= i + 1; ii++)
            {
                if (ii < lenght) continue;
                for (int jj = j - 1; jj <= j + dto.ShipLenght + 1; jj++)
                {
                    if(jj < lenght) continue;
                    if (dto.PlayerTable.Cells[ii, jj] == TilesType.Ship)
                    {
                        throw new Exception("Необходимая область уже занята другим кораблем");
                    }

                }
            }
            for(int jj = j; jj<j+dto.ShipLenght; jj++)
            {
                dto.PlayerTable.Cells[i, jj] = TilesType.Ship;
            }

            return dto;
        }




    }
}
