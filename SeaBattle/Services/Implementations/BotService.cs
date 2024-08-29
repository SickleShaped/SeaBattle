using SeaBattle.Services.Interfaces;

namespace SeaBattle.Services.Implementations;

public class BotService : Gamer, IGamer
{
    public BotService(ITableService tableService) : base(tableService)
    { }
}
