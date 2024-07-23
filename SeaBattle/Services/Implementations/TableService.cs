using SeaBattle.Consts;
using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;
using SeaBattle.Services.Interfaces;

namespace SeaBattle.Services.Implementations;

public class TableService:ITableService
{
    public bool PlaceShip(Table table, Coordinate coordinate, Ship ship)
    {
        if (CanPlaceShip(table, ship.Lenght, ship.Direction, coordinate))
        {
            for (int i = coordinate.X; i < coordinate.X + ship.Lenght; i++)
            {
                if (ship.Direction == ShipDirection.Vertical)
                    table.Cells[i, coordinate.Y] = TilesType.Ship;
                else
                    table.Cells[coordinate.Y, i] = TilesType.Ship;
            }
            return true;
        }
        return false;
    }

    public bool CheckVictory(Table table)
    {
        for (int i = 0; i < Constants.TableWidth; i++)
        {
            for (int j = 0; j < Constants.TableWidth; j++)
            {
                if (table.Cells[i, j] == TilesType.Ship && table.CellsVisibility[i, j] == TilesVisibility.Unchecked)
                    return false;
            }
        }
        return true;
    }

    public bool CanPlaceShip(Table table, int shipLenght, ShipDirection direction, Coordinate coordinate)
    {
        byte size = Constants.TableWidth;
        if (coordinate.X + shipLenght > size)
            return false;
        for (int i = coordinate.X - 1; i < coordinate.X + shipLenght + 1; i++)
        {
            if (!(i >= size || i < 0))
            {
                for (int j = coordinate.Y - 1; j <= coordinate.Y + 1; j++)
                {
                    if (j >= size || j < 0)
                        continue;
                    if (direction == ShipDirection.Vertical ? table.Cells[i, j] == TilesType.Ship : table.Cells[j, i] == TilesType.Ship)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }



}
