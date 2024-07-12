using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;

namespace SeaBattle.Services.ShipService
{
    public interface IShipService
    {
        public string FlipShip();
        public string GetCurrentShipLenght();
        public void GetFlipShipCoordinates(int cellId, ShipDirection direction, out int ii, out int jj);
    }
}
