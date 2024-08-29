
using Castle.Core.Configuration;
using Moq;
using SeaBattle.Controllers;
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

public class GameServiceTests
{
    [Fact]
    public async void GetGame_CorrentLogin_True()
    {
        //Arrange
        var mockBotService = new Mock<IGamer>();
        var mockUserService = new Mock<IUserService>();
        var mockRedisDb = new Mock<IRedisDbService>();
        var mockTableService = new Mock<ITableService>();
        var mockProducerService = new Mock<IProducerService>();

        Game returnGame = new Game { Condition = new GameCondition(new List<Ship> { new Ship(5) }), EnemyTable = new Table(), PlayerTable = new Table() };
        returnGame.PlayerTable.Cells[1, 2] = Models.Enums.TilesType.Ship;
        mockRedisDb.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(returnGame);


        GameService gameService = new GameService(mockBotService.Object, mockUserService.Object, mockRedisDb.Object, mockTableService.Object, mockProducerService.Object);
        //Act
        var recievedGame = await gameService.GetGame("11");

        //Assert
        Assert.True(recievedGame.PlayerTable.Cells[1, 2] == Models.Enums.TilesType.Ship);
    }

    [Fact]
    public async void StartGame_True()
    {
        //Arrange
        var mockBotService = new Mock<IGamer>();
        var mockUserService = new Mock<IUserService>();
        var mockRedisDb = new Mock<IRedisDbService>();
        var mockTableService = new Mock<ITableService>();
        var mockProducerService = new Mock<IProducerService>();

        Table playerTable = new Table();
        playerTable.Cells[1, 2] = TilesType.Ship;

        Game returnGame = new Game { Condition = new GameCondition(new List<Ship> { new Ship(2) }), EnemyTable = new Table(), PlayerTable = playerTable };
        returnGame.Condition.Ships[0].IsPlaced = true;
        mockRedisDb.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(returnGame);

        GameService gameService = new GameService(mockBotService.Object, mockUserService.Object, mockRedisDb.Object, mockTableService.Object, mockProducerService.Object);
        //Act

        var game = await gameService.StartGame("11");
        //Assert
        Assert.True(game.Condition.GameState == GameState.Game);
        
    }

    [Fact]
    public async void MakeTurn_CorrectDto_True()
    {
        //Arrange
        var mockBotService = new Mock<IGamer>();
        var mockUserService = new Mock<IUserService>();
        var mockRedisDb = new Mock<IRedisDbService>();
        var mockTableService = new Mock<ITableService>();
        var mockProducerService = new Mock<IProducerService>();

        MakeTurnDto dto = new MakeTurnDto() { CellId = 12};


        Game returnGame = new Game { Condition = new GameCondition(new List<Ship> { new Ship(2) }), EnemyTable = new Table(), PlayerTable = new Table() };
        returnGame.Condition.GameState = GameState.Game;
        mockRedisDb.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(returnGame);
        mockUserService.Setup(x=>x.Shoot(It.IsAny<Table>(), It.IsAny<Coordinate>())).Returns(ShootResult.Miss);
        mockBotService.Setup(x=>x.Shoot(It.IsAny<Table>(), It.IsAny<Coordinate>())).Returns(ShootResult.Miss);

        GameService gameService = new GameService(mockBotService.Object, mockUserService.Object, mockRedisDb.Object, mockTableService.Object, mockProducerService.Object);
        //Act
        var game = await gameService.MakeTurn("11", dto);

        //Assert
        Assert.True(game != null);
    }

    [Fact]
    public async void AutoMakeTablePlayer_Correct_True()
    {
        //Arrange
        var mockBotService = new Mock<IGamer>();
        var mockUserService = new Mock<IUserService>();
        var mockRedisDb = new Mock<IRedisDbService>();
        var mockTableService = new Mock<ITableService>();
        var mockProducerService = new Mock<IProducerService>();

        mockUserService.Setup(x => x.AutoMakeTable(It.IsAny<List<Ship>>())).Returns(new Table());
        Game returnGame = new Game { Condition = new GameCondition(new List<Ship> { new Ship(2)}), EnemyTable = new Table(), PlayerTable = new Table() };
        mockRedisDb.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(returnGame);

        GameService gameService = new GameService(mockBotService.Object, mockUserService.Object, mockRedisDb.Object, mockTableService.Object, mockProducerService.Object);
        //Act

        var game = await gameService.AutoMakeTablePlayer("11");
        //Assert
        Assert.True(game.Condition.LastRequestResult == LastRequestResult.Ok);
    }


}
