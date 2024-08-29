using Castle.Core.Logging;
using Confluent.Kafka.Admin;
using Moq;
using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;
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
    public async void PlaceShip_Corrent_Equeals()
    {
        //Arrange
        var mockRedisDb = new Mock<IRedisDbService>();
        var mockTableService = new Mock<ITableService>();
        PlaceShipDto dto = new PlaceShipDto();
        dto.ShipId = 0;
        dto.CellId = 55;
        dto.Direction = ShipDirection.Horisontal;
        var returnedGame = new Game();

        var ships = new List<Ship> { new Ship(3) };

        returnedGame.Condition = new GameCondition(ships);
        returnedGame.PlayerTable = new Table();
        returnedGame.EnemyTable = new Table();

        Coordinate coordinate = new Coordinate(ShipDirection.Horisontal, 5, 5);
        
        mockRedisDb.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(returnedGame);
        mockTableService.Setup(x => x.CanPlaceShip(It.IsAny<Table>(), It.IsAny<Ship>(), It.IsAny<Coordinate>())).Returns(true);

        UserService userService = new UserService(mockTableService.Object, mockRedisDb.Object);

        //Act

        var game = await userService.PlaceShip("11", dto);
        //Assert
        Assert.True(game.PlayerTable.Cells[5,5] == TilesType.Ship );
    }

    
    
    

}
