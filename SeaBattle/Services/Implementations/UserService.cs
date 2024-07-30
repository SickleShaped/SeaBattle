using Newtonsoft.Json;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;
using SeaBattle.Services.Interfaces;
using StackExchange.Redis;

namespace SeaBattle.Services.Implementations;

public class UserService : Gamer, IUserService
{
    private readonly IRedisDbService _db;

    public UserService(ITableService tableService, IRedisDbService db) : base(tableService) =>
    
        _db = db;
    

    public async Task<Game> PlaceShip(string login, PlaceShipDto dto)
    {
        Game game = await _db.Get(login);

        if (game.Condition.GameState != GameState.PlacingShips)
        {
            game.Condition.LastRequestResult = LastRequestResult.GameAlreadyStarted;
            return game;
        }


        int cell_x = dto.CellId % 10;
        int cell_y = (dto.CellId - cell_x) / 10;

        Coordinate coordinate = new Coordinate(dto.Direction, cell_x, cell_y);



        var ship = game.Condition.Ships[dto.ShipId];
        ship.Direction = dto.Direction;

        if (_tableService.CanPlaceShip(game.PlayerTable, ship, coordinate))
        {
            for (int i = coordinate.X; i < coordinate.X + ship.Lenght; i++)
            {
                if (dto.Direction == ShipDirection.Vertical)
                    game.PlayerTable.Cells[i, coordinate.Y] = TilesType.Ship;
                else
                    game.PlayerTable.Cells[coordinate.Y, i] = TilesType.Ship;
            }
            game.Condition.Ships[dto.ShipId].IsPlaced = true;
            await _db.Set(login, game);
            game.Condition.LastRequestResult = LastRequestResult.Ok;
            return game;
        }
        game.Condition.LastRequestResult = LastRequestResult.WrongCell;
        return game;

    }

}
