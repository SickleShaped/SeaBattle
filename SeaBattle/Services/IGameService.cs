using SeaBattle.Models.Tables;
using SeaBattle.Models;
using SeaBattle.Models.Enums;

namespace SeaBattle.Services
{
    public interface IGameService
    {
        public string GetInitialData();
        public string PlaceShip(string json);
        public List<Ship> GetShips();
        public Table GetTable(byte size, bool belongsPlayers);

    }
        

}
