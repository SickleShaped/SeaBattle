
using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;

namespace SeaBattle.Services.Game
{
    public interface IGameService
    {
        public void InitGameData();
        public List<Ship> GetShips();
        public string GetGameData();
        public string RestartGame();
        public void StartGame();
        public void MakeTurn(string json);
        public bool CheckVictory(Table table, GameCondition condition);


    }


}
