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

        //Подумал, что о направлении корабля беку знать бессмысленно, ведь он сохраняет только точки
        //public ShipDirection Direction { get; set; } = ShipDirection.Horisontal;

        public Ship(byte Lenght)
        {
            this.Lenght = Lenght;
        }

        /*public void ChangeDirection()
        {
            if(Direction == ShipDirection.Horisontal)
            {
                Direction = ShipDirection.Vertical;
            }
            else
            {
                Direction = ShipDirection.Horisontal;
            }
            
        }*/
    }

    
}
