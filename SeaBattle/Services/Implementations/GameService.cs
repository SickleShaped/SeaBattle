using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SeaBattle.Consts;
using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;
using SeaBattle.Services.Interfaces;
using StackExchange.Redis;
using System.Diagnostics;
using System.Drawing;

namespace SeaBattle.Services.Implementations;

public class GameService : IGameService
{
    private readonly IUserService _userService;
    private readonly IGamer _botService;
    private readonly ITableService _tableService;
    private readonly IRedisDbService db;
    private readonly IRabbitMqService _rabbit;

    public GameService(IGamer botService, IUserService userService, IRedisDbService db, ITableService tableService, IRabbitMqService rabbit)
    {
        this.db = db;
        _botService = botService;
        _userService = userService;
        _tableService = tableService;
        _rabbit = rabbit;
    }


    public async Task<Game> GetGame(string login)
    {
        return await db.Get(login);
    }

    public async Task<Game> RestartGame(string login)
    {
        await db.Delete(login);
        return await db.Get(login);
    }

    public async Task<Game> StartGame(string login)
    {
        var game = await db.Get(login);
        bool allShipsPlaced = true;
        foreach (var ship in game.Condition.Ships)
        {
            if (ship.IsPlaced == false)
            {
                game.Condition.LastRequestResult = LastRequestResult.ShipsArentPlaced;
                return game;
            }   
        }
        game.Condition.GameState = GameState.Game;
        game.Condition.LastRequestResult = LastRequestResult.Ok;
        await db.Set(login, game);
        return game;

    }


    public async Task<Game> MakeTurn(string login, MakeTurnDto dto)
    {
        Game game = (await db.Get(login));
        if (game.Condition.GameState != GameState.Game)
        {
            game.Condition.LastRequestResult = LastRequestResult.ShipsArentPlaced;
            return game;
        }
        int cellId = Math.Abs(dto.CellId)-1;
        int y = cellId % 10;
        int x = (cellId-y) / 10;
        Coordinate coordinate = new Coordinate {X = x, Y = y };

        if (await PlayerTurn(login, game, coordinate))
        {
            await EnemyTurn(login, game);
        }
        return game;
    }

    
    public async Task<Game> AutoMakeTablePlayer(string login)
    {
        var game = await db.Get(login);
        if (game.Condition.GameState != GameState.PlacingShips)
        {
            game.Condition.LastRequestResult = LastRequestResult.ShipsArentPlaced;
            return game;
        }
        game.PlayerTable = _userService.AutoMakeTable(game.Condition.Ships);
        foreach(var ship in game.Condition.Ships)
        {
            ship.IsPlaced = true;
        }
        game.Condition.LastRequestResult = LastRequestResult.Ok;
        
        await db.Set(login, game);
        return game;
    }

    private async Task<bool> PlayerTurn(string login, Game game, Coordinate coordinate)
    {
        RabbitMessage rabbitMessage = new RabbitMessage();
        rabbitMessage.Login = login;
        rabbitMessage.Player = PlayerEnum.Player;
        rabbitMessage.Coordinate = coordinate;

        ShootResult shootResult = _userService.Shoot(game.EnemyTable, coordinate);
        switch (shootResult)
        {
            case ShootResult.Hit:
                if (_tableService.CheckVictory(game.EnemyTable))
                {
                    game.Condition.GameState = GameState.PlayerWin;
                }
                game.Condition.LastRequestResult = LastRequestResult.Ok;
                await db.Set(login, game);
                
        
                _rabbit.SendMessage(rabbitMessage);
                return false;

            case ShootResult.SamePointShooted:
                game.Condition.LastRequestResult = LastRequestResult.SamePointShooted;
                return false;

            default:
                game.Condition.LastRequestResult = LastRequestResult.Ok;
                _rabbit.SendMessage(rabbitMessage);

                return true;
        }
    }

    private async Task EnemyTurn(string login, Game game)
    {
        bool BotNotMiss = true;
        Coordinate lastBotShoot = new Coordinate();
        if (game.Condition.lastBotShoot != null) { lastBotShoot = game.Condition.lastBotShoot; }
        Coordinate coordinateBotShoot = new Coordinate();
        while (BotNotMiss)
        {
            

            Random random = new Random();

            if (game.PlayerTable.Cells[lastBotShoot.X, lastBotShoot.Y] == TilesType.Ship &&
                ((lastBotShoot.X - 1 >= 0 && game.PlayerTable.CellsVisibility[lastBotShoot.X - 1, lastBotShoot.Y] == TilesVisibility.Unchecked)
                || (lastBotShoot.X < Constants.TableWidth -1 && game.PlayerTable.CellsVisibility[lastBotShoot.X + 1, lastBotShoot.Y] == TilesVisibility.Unchecked)
                || (lastBotShoot.Y - 1 >= 0 && game.PlayerTable.CellsVisibility[lastBotShoot.X, lastBotShoot.Y-1] == TilesVisibility.Unchecked)
                || (lastBotShoot.Y < Constants.TableWidth -1 && game.PlayerTable.CellsVisibility[lastBotShoot.X, lastBotShoot.Y+1] == TilesVisibility.Unchecked)))
            {
                switch (random.Next(1, 5))
                {
                    case 1:
                        if(lastBotShoot.X < Constants.TableWidth-1)
                        {
                            coordinateBotShoot.X = lastBotShoot.X + 1;
                            coordinateBotShoot.Y = lastBotShoot.Y;
                        }
                        break;

                    case 2:
                        if(lastBotShoot.X - 1 >= 0)
                        {
                            coordinateBotShoot.X = lastBotShoot.X - 1;
                            coordinateBotShoot.Y = lastBotShoot.Y;
                        } 
                        break;

                    case 3:
                        if(lastBotShoot.Y < Constants.TableWidth-1)
                        {
                            coordinateBotShoot.X = lastBotShoot.X;
                            coordinateBotShoot.Y = lastBotShoot.Y + 1;
                        }
                        break;

                    case 4:
                        if(lastBotShoot.Y - 1 >= 0)
                        {
                            coordinateBotShoot.X = lastBotShoot.X;
                            coordinateBotShoot.Y = lastBotShoot.Y - 1;
                        }
                        break;
                }
            }
            else
            {
                coordinateBotShoot.X = random.Next(0, Constants.TableWidth);
                coordinateBotShoot.Y = random.Next(0, Constants.TableWidth);
            }

            RabbitMessage rabbitMessage = new RabbitMessage();
            rabbitMessage.Login = login;
            rabbitMessage.Player = PlayerEnum.Bot;
            rabbitMessage.Coordinate = coordinateBotShoot;

            ShootResult result = _botService.Shoot(game.PlayerTable, coordinateBotShoot);

            switch (result)
            {
                case ShootResult.Hit:
                    lastBotShoot.X = coordinateBotShoot.X;
                    lastBotShoot.Y = coordinateBotShoot.Y;
                    if (_tableService.CheckVictory(game.PlayerTable))
                    {
                        game.Condition.GameState = GameState.EnemyWin;
                        return;
                    }
                    _rabbit.SendMessage(rabbitMessage);
                    break;

                case ShootResult.Miss:
                    BotNotMiss = false;
                    _rabbit.SendMessage(rabbitMessage);
                    break;
            }
        }
        game.Condition.lastBotShoot = lastBotShoot;
        await db.Set(login, game);
    }
    
}
