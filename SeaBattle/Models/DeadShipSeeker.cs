using Microsoft.AspNetCore.Authentication.Cookies;
using SeaBattle.Consts;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;

namespace SeaBattle.Models;

public class DeadShipSeeker
{
    public int LowerX {  get; set; }
    public int UpperX { get; set; }
    public int LowerY { get; set; }
    public int UpperY { get; set; }

    public bool ShipIsDead {  get; set; }

    public DeadShipSeeker(Coordinate coordinate)
    {
        LowerX = coordinate.X;
        UpperX = coordinate.X;
        UpperY = coordinate.Y;
        LowerY = coordinate.Y;
        ShipIsDead = true;
    }

}
