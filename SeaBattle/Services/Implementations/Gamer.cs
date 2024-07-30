using SeaBattle.Consts;
using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;
using SeaBattle.Services.Interfaces;

namespace SeaBattle.Services.Implementations;
public abstract class Gamer : IGamer
{
    protected readonly ITableService _tableService;

    public Gamer(ITableService tableService)
    {
        _tableService = tableService;
    }

    public ShootResult Shoot(Table table, Coordinate coordinate)
    {
        if (table.CellsVisibility[coordinate.X, coordinate.Y] == TilesVisibility.Checked) return ShootResult.SamePointShooted;
        table.CellsVisibility[coordinate.X, coordinate.Y] = TilesVisibility.Checked;
        if (table.Cells[coordinate.X, coordinate.Y] == TilesType.Ship)
        {
            CheckShipIsDead(table, coordinate);

            return ShootResult.Hit;
        }
        else return ShootResult.Miss;
    }

    public Table AutoMakeTable(List<Ship> ships)
    {
        Table table = new Table();

        foreach (var ship in ships)
        {
            while (true)
            {
                if (PlaceShipRandom(ship, table))
                    break;
            }
        }
        return table;
    }

    private bool PlaceShipRandom(Ship ship, Table table)
    {
        int size = Constants.TableWidth;
        Random rand = new Random();
        int x = rand.Next(0, size);
        int y = rand.Next(0, size);

        ship.Direction = (ShipDirection)rand.Next(0, 2);

        Coordinate coordinate = new Coordinate(ship.Direction, x, y);


        if (_tableService.CanPlaceShip(table, ship, coordinate))
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

    public Table CheckShipIsDead(Table table, Coordinate coordinate)
    {
        DeadShipSeeker seeker = new DeadShipSeeker(coordinate);
        if (table.CellsVisibility[coordinate.X, coordinate.Y] == TilesVisibility.Unchecked) return table;

        CheckDown(ref seeker, table, coordinate);
        CheckUp(ref seeker, table, coordinate);
        CheckRight(ref seeker, table, coordinate);
        CheckLeft(ref seeker, table, coordinate);

        if (seeker.ShipIsDead) CheckAllAroundShip(table, seeker);
        return table;
    }

    private void CheckAllAroundShip(Table table, DeadShipSeeker seeker)
    {
        for (int i = seeker.LowerX - 1; i <= seeker.UpperX + 1; i++)
        {
            if (i >= 0 && i < Constants.TableWidth)
            {
                for (int j = seeker.LowerY - 1; j <= seeker.UpperY + 1; j++)
                {
                    if (j >= 0 && j < Constants.TableWidth)
                        table.CellsVisibility[i, j] = TilesVisibility.Checked;
                }
            }
        }
    }
    private void CheckDown(ref DeadShipSeeker seeker, Table table, Coordinate coordinate)
    {
        while (seeker.UpperX < Constants.TableWidth - 1) //вниз
        {
            if (table.Cells[seeker.UpperX + 1, coordinate.Y] == TilesType.Ship)
            {
                if (table.CellsVisibility[seeker.UpperX + 1, seeker.UpperY] == TilesVisibility.Unchecked)
                {
                    seeker.ShipIsDead = false;
                    break;
                }
                else { seeker.UpperX++; }
            }
            else { break; }
        }
    }
    private void CheckUp(ref DeadShipSeeker seeker, Table table, Coordinate coordinate)
    {
        while (seeker.LowerX > 0)
        {
            if (table.Cells[seeker.LowerX - 1, coordinate.Y] == TilesType.Ship)
            {
                if (table.CellsVisibility[seeker.LowerX - 1, seeker.UpperY] == TilesVisibility.Unchecked)
                {
                    seeker.ShipIsDead = false;
                    break;
                }
                else { seeker.LowerX--; }
            }
            else { break; }
        }
    }
    private void CheckLeft(ref DeadShipSeeker seeker, Table table, Coordinate coordinate)
    {
        while (seeker.LowerY > 0)
        {
            if (table.Cells[coordinate.X, seeker.LowerY - 1] == TilesType.Ship)
            {
                if (table.CellsVisibility[coordinate.X, seeker.LowerY - 1] == TilesVisibility.Unchecked)
                {
                    seeker.ShipIsDead = false;
                    break;
                }
                else { seeker.LowerY--; }
            }
            else { break; }
        }
    }
    private void CheckRight(ref DeadShipSeeker seeker, Table table, Coordinate coordinate)
    {
        while (seeker.UpperY < Constants.TableWidth - 1)
        {
            if (table.Cells[coordinate.X, seeker.UpperY + 1] == TilesType.Ship)
            {
                if (table.CellsVisibility[coordinate.X, seeker.UpperY + 1] == TilesVisibility.Unchecked)
                {
                    seeker.ShipIsDead = false;
                    break;
                }
                else { seeker.UpperY++; }
            }
            else { break; }
        }
    }
}