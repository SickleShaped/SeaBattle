using SeaBattle.Models.AuxilaryModels;

namespace SeaBattle.Services.ShipService
{
    public interface IShipService
    {
        public string FlipShip();

        public void PlaceShip(string json);

        public string GetCurrentShipLenght();

    }
}
