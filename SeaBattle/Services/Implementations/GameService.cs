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
    private readonly IRedisDbService _db;
    private readonly ProducerService _producerService;

    public GameService(IGamer botService, IUserService userService, IRedisDbService db, ITableService tableService, ProducerService producerService)
    {
        _db = db;
        _botService = botService;
        _userService = userService;
        _tableService = tableService;
        _producerService = producerService;
    }


    public async Task<Game> GetGame(string login)
    {
        return await _db.Get(login);
    }

    public async Task<Game> RestartGame(string login)
    {
        await _db.Delete(login);
         //_rabbit.Clear(login);
        return await _db.Get(login);
    }

    public async Task<Game> StartGame(string login)
    {
        var game = await _db.Get(login);
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
        await _db.Set(login, game);
        return game;

    }


    public async Task<Game> MakeTurn(string login, MakeTurnDto dto)
    {
        Game game = (await _db.Get(login));
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
        var game = await _db.Get(login);
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
        
        await _db.Set(login, game);
        return game;
    }

    private async Task<bool> PlayerTurn(string login, Game game, Coordinate coordinate)
    {
        KafkaMessage kafkaMessage = new KafkaMessage();
        kafkaMessage.Login =  login;
        kafkaMessage.Player = PlayerEnum.Player;
        kafkaMessage.Coordinate = coordinate;

        ShootResult shootResult = _userService.Shoot(game.EnemyTable, coordinate);
        switch (shootResult)
        {
            case ShootResult.Hit:
                if (_tableService.CheckVictory(game.EnemyTable))
                {
                    game.Condition.GameState = GameState.PlayerWin;
                }
                game.Condition.LastRequestResult = LastRequestResult.Ok;
                await _db.Set(login, game);

                _producerService.ProduceAsync(login, JsonConvert.SerializeObject(kafkaMessage));
                return false;

            case ShootResult.SamePointShooted:
                game.Condition.LastRequestResult = LastRequestResult.SamePointShooted;
                return false;

            default:
                game.Condition.LastRequestResult = LastRequestResult.Ok;
                _producerService.ProduceAsync(login, JsonConvert.SerializeObject(kafkaMessage));
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

            KafkaMessage kafkaMessage = new KafkaMessage();
            kafkaMessage.Login = login;
            kafkaMessage.Player = PlayerEnum.Bot;
            kafkaMessage.Coordinate = coordinateBotShoot;

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
                    _producerService.ProduceAsync(login, JsonConvert.SerializeObject(kafkaMessage));
                    break;

                case ShootResult.Miss:
                    BotNotMiss = false;
                    _producerService.ProduceAsync(login, JsonConvert.SerializeObject(kafkaMessage));
                    break;
            }
        }
        game.Condition.lastBotShoot = lastBotShoot;
        await _db.Set(login, game);
    }
    
}
