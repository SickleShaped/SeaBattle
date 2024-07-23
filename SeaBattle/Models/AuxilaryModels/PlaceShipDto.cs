using SeaBattle.Models.Enums;

namespace SeaBattle.Models.AuxilaryModels
{
    /// <summary>
    /// DTO с клеткой для помещения корабля
    /// </summary>
    public class PlaceShipDto
    {
        public int CellId { get; set; }
        public int ShipId { get; set; }
        public ShipDirection Direction { get; set; }
    }
}
