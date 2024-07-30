using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;
using SeaBattle.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SeaBattle.Tests;

public class TableServiceTests
{
    private readonly TableService _tableService;
    public TableServiceTests()
    {
        _tableService = new TableService();
    }

    [Fact]
    public void PlaceShip_CorrectValues_True()
    {
        //arrange
        Table table = new Table();
        table.Cells[0, 0] = TilesType.Ship;

        table.Cells[4, 0] = TilesType.Ship;
        Coordinate coordinate = new Coordinate() { X = 3, Y = 0 };
        //act
        Ship ship = new Ship(3);
        ship.Direction = ShipDirection.Horisontal;
        bool result = _tableService.CanPlaceShip(table,ship, coordinate);
        //assert
        Assert.True(result);
    }
    [Fact]
    public void PlaceShip_WrongValues_False()
    {
        //arrange
        Table table = new Table();
        Coordinate coordinate = new Coordinate() { X = 9, Y = 9 };
        //act
        Ship ship = new Ship(3);
        ship.Direction = ShipDirection.Horisontal;
        bool result = _tableService.CanPlaceShip(table, ship, coordinate);
        //assert
        Assert.False(result);
    }
    [Fact]
    public void CanPlaceShip_CorrectValues_True()
    {
        //arrange
        Table table = new Table();
        table.Cells[0, 0] = TilesType.Ship;

        table.Cells[4, 0] = TilesType.Ship;
        Coordinate coordinate = new Coordinate() { X = 3, Y = 0 };
        //act
        Ship ship = new Ship(3);
        ship.Direction = ShipDirection.Horisontal;
        bool result = _tableService.CanPlaceShip(table, ship, coordinate);
        //assert
        Assert.True(result);
    }
    [Fact]
    public void CanPlaceShip_WrongValues_False()
    {
        //arrange
        Table table = new Table();
        Coordinate coordinate = new Coordinate() {X=9, Y=9 };
        //act
        Ship ship = new Ship(3);
        ship.Direction = ShipDirection.Horisontal;
        bool result = _tableService.CanPlaceShip(table, ship, coordinate);
        //assert
        Assert.False(result);
    }
    [Fact]
    public void CheckVictory_Victory_True()
    {
        //arrange
        Table table = new Table();
        table.Cells[1, 3] = TilesType.Ship;
        table.Cells[2, 3] = TilesType.Ship;
        table.Cells[3, 3] = TilesType.Ship;
        table.CellsVisibility[1, 3] = TilesVisibility.Checked;
        table.CellsVisibility[2, 3] = TilesVisibility.Checked;
        table.CellsVisibility[3, 3] = TilesVisibility.Checked;
        //act
        bool result = _tableService.CheckVictory(table);

        //assert
        Assert.True(result);

    }
    [Fact]
    public void CheckVictory_NotVictory_False()
    {
        //arrange
        Table table = new Table();
        table.Cells[1, 3] = TilesType.Ship;
        table.Cells[2, 3] = TilesType.Ship;
        table.Cells[3, 3] = TilesType.Ship;
        table.CellsVisibility[1, 3] = TilesVisibility.Checked;
        table.CellsVisibility[2, 3] = TilesVisibility.Checked;
        //act
        bool result = _tableService.CheckVictory(table);

        //assert
        Assert.False(result);
    }
}
