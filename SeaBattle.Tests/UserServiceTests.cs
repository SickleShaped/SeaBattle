using Moq;
using SeaBattle.Models;
using SeaBattle.Services.Implementations;
using SeaBattle.Services.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SeaBattle.Tests;

public class UserServiceTests
{
    [Fact]
    public void AutoMakeTable_AllShipsPlaced_True()
    {
        /*
        // Arrange
        var mockDb = new Mock<IDatabase>();
        var mockTable = new Mock<ITableService>();

        UserService userService = new UserService(mockTable.Object, mockDb.Object);
        //Act
        List<Ship> ships = new List<Ship>()
        {
            new Ship(4), new Ship(3), new Ship(3), new Ship(2),new Ship(2),
            new Ship(2), new Ship(1), new Ship(1), new Ship(1), new Ship(1)
        };
        var game = userService.AutoMakeTable(ships);
        
        bool AllShipsPlaced = true;
        foreach (Ship ship in ships)
        {
            if(!ship.IsPlaced)
                AllShipsPlaced = false;
        }
        // Assert
        Assert.True(AllShipsPlaced);
        */

    }
}
