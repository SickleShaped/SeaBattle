using SeaBattle.Models.Enums;

namespace SeaBattle.Models
{
    /// <summary>
    /// Класс, представляющий корабль
    /// </summary>
    public class Ship
    {
        /// <summary>
        /// Длина корабля в клеточках
        /// </summary>
        public byte Lenght { get; set; }


        public Ship(byte Lenght)
        {
            this.Lenght = Lenght;
        }

    }

    
}
