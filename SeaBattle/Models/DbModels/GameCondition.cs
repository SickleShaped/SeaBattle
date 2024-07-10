namespace SeaBattle.Models.DbModels
{
    public class GameCondition
    {
        public int EnemyStore { get;set; }
        public int PlayerStore { get; set; }
        public int WinStore { get; set; }
        public bool IsGameStarted { get; set; }
        public bool IsGameFinished { get; set; }
        public bool IsPlayerTurn { get; set; }
        public List<Ship> Ships { get; set; }
     
        public GameCondition(List<Ship> ships)
        {
            Ships = ships;
            WinStore = 0;
            PlayerStore = 0;
            EnemyStore = 0;

            IsGameStarted = false;
            IsGameFinished = false;
            IsPlayerTurn = true;

            

            foreach(var ship in Ships)
            {
                WinStore += ship.Lenght;
            }
        }

        public GameCondition() { }
    }
}
