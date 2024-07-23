using SeaBattle.Services.Interfaces;

namespace SeaBattle.Services.Implementations;

public class BotService : Gamer
{
    public BotService(ITableService tableService) : base(tableService)
    { }
}
