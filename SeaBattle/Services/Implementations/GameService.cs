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


    public async Task<string> GetGame(string login)
    {
        var response = await db.StringGetAsync($"Game_{login}");
        if (response == RedisValue.Null)
            return JsonConvert.SerializeObject(await InitGameData(login));
        Game game = JsonConvert.DeserializeObject<Game>(response);
        return response;
    }

    public async Task<string> RestartGame(string login)
    {
        var game = JsonConvert.SerializeObject(await InitGameData(login));
        return game;
    }

    public async Task StartGame(string login)
    {
        var response = await db.StringGetAsync($"Game_{login}");
        var game = JsonConvert.DeserializeObject<Game>(response);
        bool allShipsPlaced = true;
        foreach (var ship in game.Condition.Ships)
        {
            if (ship.IsPlaced == false)
                throw new Exception("Не все норабли поставлены!");
        }
        game.Condition.GameState = GameState.Game;
        await db.StringSetAsync($"Game_{login}", JsonConvert.SerializeObject(game));

    }


    public async Task MakeTurn(string login, MakeTurnDto dto)
    {
        Game game = JsonConvert.DeserializeObject<Game>(await db.StringGetAsync($"Game_{login}"));
        if (game.Condition.GameState != GameState.Game)
            throw new Exception("Игра не началась или уже закончилась!");
        int cellId = dto.CellId - 100 ;
        int y = cellId % 10;
        int x = (cellId-y) / 10;

        Coordinate coordinate = new Coordinate(x, y);
        #region UserTurn
            ShootResult shootResult = _userService.Shoot(game.EnemyTable, coordinate);
            if (shootResult == ShootResult.SamePointShooted)
            {
                return;
            }
            if (_tableService.CheckVictory(game.EnemyTable))
            {
                game.Condition.GameState = GameState.PlayerWin;
            await db.StringSetAsync($"Game_{login}", JsonConvert.SerializeObject(game));
            return;
            }
            if (shootResult == ShootResult.Hit)
            {
                await db.StringSetAsync($"Game_{login}", JsonConvert.SerializeObject(game));
                return;
            }
        #endregion
        #region EnemyTurn
            bool BotHitOrRepeated = true;
            while (BotHitOrRepeated)
            {
                Random random = new Random();
                Coordinate coordinateBotShoot = new Coordinate(random.Next(0, Constants.TableWidth), random.Next(0, Constants.TableWidth));
                if (_botService.Shoot(game.PlayerTable, coordinateBotShoot) == ShootResult.Miss)
                    BotHitOrRepeated = false;
                else
                {
                    if (_tableService.CheckVictory(game.PlayerTable))
                    {
                        game.Condition.GameState = GameState.EnemyWin;
                        return;
                    }

                }
            }
        #endregion


        await db.StringSetAsync($"Game_{login}", JsonConvert.SerializeObject(game));
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
    public async Task AutoMakeTablePlayer(string login)
    {
        var game = JsonConvert.DeserializeObject<Game>(await db.StringGetAsync($"Game_{login}"));
        if (game.Condition.GameState != GameState.PlacingShips)
            throw new Exception("Нельзя переставлять корабли во время игры!");
        game.PlayerTable = _userService.AutoMakeTable(game.Condition.Ships);
        foreach(var ship in game.Condition.Ships)
        {
            ship.IsPlaced = true;
        }
        await db.StringSetAsync($"Game_{login}", JsonConvert.SerializeObject(game));
    }
    
}
