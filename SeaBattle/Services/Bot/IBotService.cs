using SeaBattle.Models;
using SeaBattle.Models.Tables;

namespace SeaBattle.Services.Bot
{
    public interface IBotService
    {
        public Table MakeTable(List<Ship> ships);
    }
}
