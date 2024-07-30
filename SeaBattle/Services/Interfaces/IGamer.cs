using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;

namespace SeaBattle.Services.Interfaces;

public interface IGamer
{
    ShootResult Shoot(Table table, Coordinate coordinate);
    Table AutoMakeTable(List<Ship> ships);
}
