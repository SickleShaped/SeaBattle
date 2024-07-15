using SeaBattle.Models.Enums;

namespace SeaBattle.Models.AuxilaryModels
{
    /// <summary>
    /// DTO конкретного корябля
    /// </summary>
    public class CurrentShipDto
    {
        public ShipDirection Direction { get; set; }
        public byte ShipLenght { get; set; }
    }
}
