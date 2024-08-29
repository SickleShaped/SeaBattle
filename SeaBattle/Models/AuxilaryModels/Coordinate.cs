using SeaBattle.Models.Enums;

namespace SeaBattle.Models.AuxilaryModels;

public class Coordinate
{
    public int X {  get; set; }
    public int Y {  get; set; }

    public Coordinate(){}

    public Coordinate(ShipDirection direction, int x, int y)
    {
        if (direction == ShipDirection.Horisontal)
        {
            X = x;
            Y = y;
        }
        else
        {
            X = y;
            Y = x;
        }
    }
}
