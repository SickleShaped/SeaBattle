using SeaBattle.Models.Enums;
using SeaBattle.Models.Tables;

namespace SeaBattle.Models.AuxilaryModels
{
    public class PlaceShipDto
    {
        public Table PlayerTable { get; set; }
        public byte ShipLenght { get; set; }
        public ShipDirection Direction { get; set; }

        public byte Cell { get; set; }
    }
}
