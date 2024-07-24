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
    private readonly UserService _userService;
    private readonly BotService _botService;
    private readonly ITableService _tableService;
    private readonly IDatabase db;

    public GameService(BotService botService, UserService userService, IDatabase db, ITableService tableService)
    {
        this.db = db;
        _botService = botService;
        _userService = userService;
        _tableService = tableService;
    }


    public async Task<Game> GetGame(string login)
    {
        var response = await db.StringGetAsync($"Game_{login}");
        if (response == RedisValue.Null)
            return await InitGameData(login);
        Game game = JsonConvert.DeserializeObject<Game>(response);
        return game;
    }

    public async Task<Game> RestartGame(string login)
    {
        var game = await InitGameData(login);
        return game;
    }

    public async Task<Game> StartGame(string login)
    {
        var response = await db.StringGetAsync($"Game_{login}");
        var game = JsonConvert.DeserializeObject<Game>(response);
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
        await db.StringSetAsync($"Game_{login}", JsonConvert.SerializeObject(game));
        return game;

    }


    public async Task<Game> MakeTurn(string login, MakeTurnDto dto)
    {
        Game game = JsonConvert.DeserializeObject<Game>(await db.StringGetAsync($"Game_{login}"));
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

    private async Task<Game> InitGameData(string login)
    {

        var ships = new List<Ship>
        {
            new Ship(4), new Ship(3), new Ship(3), new Ship(2), new Ship(2),
            new Ship(2), new Ship(1), new Ship(1), new Ship(1), new Ship(1)
        };

        Game game = new Game()
        {
            PlayerTable = new Table(),
            EnemyTable = _botService.AutoMakeTable(ships),
            Condition = new GameCondition(ships)
            {
                GameState = GameState.PlacingShips,
            }
        };

        await db.StringSetAsync($"Game_{login}", JsonConvert.SerializeObject(game));
        return game;
    }
    public async Task<Game> AutoMakeTablePlayer(string login)
    {
        var game = JsonConvert.DeserializeObject<Game>(await db.StringGetAsync($"Game_{login}"));
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
        
        await db.StringSetAsync($"Game_{login}", JsonConvert.SerializeObject(game));
        return game;
    }

    private async Task<bool> PlayerTurn(string login, Game game, Coordinate coordinate)
    {
        ShootResult shootResult = _userService.Shoot(game.EnemyTable, coordinate);
        switch (shootResult)
        {
            case ShootResult.Hit:
                if (_tableService.CheckVictory(game.EnemyTable))
                {
                    game.Condition.GameState = GameState.PlayerWin;
                }
                game.Condition.LastRequestResult = LastRequestResult.Ok;
                await db.StringSetAsync($"Game_{login}", JsonConvert.SerializeObject(game));
                return false;

            case ShootResult.SamePointShooted:
                game.Condition.LastRequestResult = LastRequestResult.SamePointShooted;
                return false;

            default:
                game.Condition.LastRequestResult = LastRequestResult.Ok;
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
                || (lastBotShoot.Y < Constants.TableWidth -1 && game.PlayerTable.CellsVisibility[lastBotShoot.X, lastBotShoot.Y+1] == TilesVisibility.Unchecked)

                ))
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
                    break;

                case ShootResult.Miss:
                    BotNotMiss = false;
                    break;
            }
        }
        game.Condition.lastBotShoot = lastBotShoot;
        await db.StringSetAsync($"Game_{login}", JsonConvert.SerializeObject(game));
    }
    
}
