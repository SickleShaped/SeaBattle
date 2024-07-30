using SeaBattle.Consts;
using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;

namespace SeaBattle.Services.Interfaces;

public interface ITableService
{
    bool CheckVictory(Table table);
    bool PlaceShip(Table table, Coordinate coordinate, Ship ship );
    bool CanPlaceShip(Table table, Ship ship, Coordinate coordinate);
}
