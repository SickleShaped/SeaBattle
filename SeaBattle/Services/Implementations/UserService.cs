using Newtonsoft.Json;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;
using SeaBattle.Services.Interfaces;
using StackExchange.Redis;

namespace SeaBattle.Services.Implementations;

public class UserService : Gamer
{
    private readonly BotService _botService;
    private readonly ITableService _tableService;
    private readonly IDatabase _db;

    public UserService(BotService botService, ITableService tableService, IDatabase db) : base(tableService)
    {
        _botService = botService;
        _tableService = tableService;
        _db = db;
    }

    public async Task PlaceShip(string login, PlaceShipDto dto)
    {
        Game game = JsonConvert.DeserializeObject<Game>(await _db.StringGetAsync($"Game_{login}"));

        if (game.Condition.GameState != GameState.PlacingShips)
            throw new Exception("Игра уже началась");


        int cell_x = dto.CellId % 10;
        int cell_y = (dto.CellId - cell_x) / 10;

        Coordinate coordinate = new Coordinate(dto.Direction, cell_x, cell_y);



        var ship = game.Condition.Ships[dto.ShipId];

        if (_tableService.CanPlaceShip(game.PlayerTable, ship.Lenght, dto.Direction, coordinate))
        {
            for (int i = coordinate.X; i < coordinate.X + ship.Lenght; i++)
            {
                if (dto.Direction == ShipDirection.Vertical)
                    game.PlayerTable.Cells[i, coordinate.Y] = TilesType.Ship;
                else
                    game.PlayerTable.Cells[coordinate.Y, i] = TilesType.Ship;
            }
            game.Condition.Ships[dto.ShipId].IsPlaced = true;
            await _db.StringSetAsync($"Game_{login}", JsonConvert.SerializeObject(game));
            return;
        }
        throw new Exception("Необходимая область уже занята другим кораблем или корабль за пределами массива");

    }

}
