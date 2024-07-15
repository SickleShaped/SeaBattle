namespace SeaBattle.Models
{
    /// <summary>
    /// Класс, содержащий поля текущего состояния игры
    /// </summary>
    public class GameCondition
    {
        public int EnemyStore { get; set; }
        public int PlayerStore { get; set; }
        public int WinStore { get; set; }
        public bool IsGameStarted { get; set; }
        public bool IsGameFinished { get; set; }
        public bool IsPlayerTurn { get; set; }
        public List<Ship> Ships { get; set; }

        public bool PlayerWin { get; set; }
        public bool BotWin { get; set; }

        public GameCondition(List<Ship> ships)
        {
            Ships = ships;
            WinStore = 0;
            PlayerStore = 0;
            EnemyStore = 0;

            IsGameStarted = false;
            IsGameFinished = false;
            IsPlayerTurn = true;
            PlayerWin = false;
            BotWin = false;



            foreach (var ship in Ships)
            {
                WinStore += ship.Lenght;
            }
        }

        public GameCondition() { }
    }
}
