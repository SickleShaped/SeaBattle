using Microsoft.AspNetCore.Authentication.Cookies;
using SeaBattle.Consts;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;

namespace SeaBattle.Models
{
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

        public void CheckAllAroundShip(Table table)
        {
            for (int i = LowerX - 1; i <= UpperX + 1; i++)
            {
                if(i>=0 && i<Constants.TableWidth)
                {
                    for (int j = LowerY - 1; j <= UpperY+1; j++)
                    {
                        if (j >= 0 && j < Constants.TableWidth)
                            table.CellsVisibility[i, j] = TilesVisibility.Checked;
                    }
                } 
            }
        }
    }
}
