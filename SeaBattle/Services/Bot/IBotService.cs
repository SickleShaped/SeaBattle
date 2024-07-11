using SeaBattle.Models;

namespace SeaBattle.Services.Bot
{
    public interface IBotService
    {
        public Table MakeTable(List<Ship> ships);
    }
}
