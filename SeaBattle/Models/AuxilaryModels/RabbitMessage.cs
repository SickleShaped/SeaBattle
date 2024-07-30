using SeaBattle.Models.Enums;

namespace SeaBattle.Models.AuxilaryModels
{
    public class RabbitMessage
    {
        public PlayerEnum Player { get; set; }
        public Coordinate Coordinate { get;set; }
    }
}
