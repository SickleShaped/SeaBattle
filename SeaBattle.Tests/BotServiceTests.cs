using Moq;
using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Services.Implementations;
using SeaBattle.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SeaBattle.Tests;

public class BotServiceTests
{
    [Fact]
    public void Shoot_UncheckedCell_HitOrMiss()
    {
        //Arrange
        var mockTable = new Mock<ITableService>();
        BotService botService = new BotService(mockTable.Object);
        Table table = new Table();
        Coordinate coordinate = new Coordinate();
        coordinate.X = 1;
        coordinate.Y=2;
        //Act
        var result = botService.Shoot(table, coordinate);
        //Assert
        Assert.True(result != Models.Enums.ShootResult.SamePointShooted);
    }
    [Fact]
    public void Shoot_CheckedCell_SamePointShooted()
    {
        //Arrange
        var mockTable = new Mock<ITableService>();
        BotService botService = new BotService(mockTable.Object);
        Table table = new Table();
        table.CellsVisibility[4, 4] = Models.Enums.TilesVisibility.Checked;
        Coordinate coordinate = new Coordinate();
        coordinate.X = 4;
        coordinate.Y = 4;
        //Act
        var result = botService.Shoot(table, coordinate);
        //Assert
        Assert.True(result == Models.Enums.ShootResult.SamePointShooted);
    }

    [Fact]
    public void AutoMakeTable_AllShipsPlaced_True()
    {
        //Arrange
        var mockTable = new Mock<ITableService>();
        mockTable.Setup(x => x.CanPlaceShip(It.IsAny<Table>(), It.IsAny<Ship>(), It.IsAny<Coordinate>())).Returns(true);


        var botService = new BotService(mockTable.Object);
        List<Ship> ships = new List<Ship>()
        {
            new Ship(1)
        };
        //act
        var table = botService.AutoMakeTable(ships);
        bool hasShip = false;
        for(int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                if (table.Cells[i, j] == Models.Enums.TilesType.Ship) hasShip = true;
            }
        }
        // Assert
        Assert.True(hasShip == true);
    }


}
