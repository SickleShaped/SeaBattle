using SeaBattle.Models.Enums;

namespace SeaBattle.Models.AuxilaryModels
{
    public class RabbitMessage
    {
        public string Login { get; set; }
        public PlayerEnum Player { get; set; }
        public Coordinate Coordinate { get;set; }
    }
}
