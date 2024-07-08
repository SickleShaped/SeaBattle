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



            string result = "";
            return result;
        }


    }
}
