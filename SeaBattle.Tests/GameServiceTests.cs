
using Moq;
using SeaBattle.Controllers;
using SeaBattle.Models.AuxilaryModels;
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
    public async Task IndexViewDataMessage()
    {
        /*
        // Arrange
        var mockDb = new Mock<IDatabase>();
        var mockTable = new Mock<ITableService>();
        var mockBot = new Mock<BotService>(mockTable.Object);
        var mockUser = new Mock<IUserService>(mockTable.Object, mockDb.Object);
        GameService gameService = new GameService(mockBot.Object, mockUser.Object, mockDb.Object, mockTable.Object);

        // Act
        var result = 3;
        var i = 3;

        // Assert
        Assert.Equal(i, result);
        */

    }
}
