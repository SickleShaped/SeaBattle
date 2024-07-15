using System.Security.Cryptography.X509Certificates;

namespace SeaBattle.Models.AuxilaryModels
{
    /// <summary>
    /// DTO инициализируемой игры
    /// </summary>
    public class InitGameModelDto
    {
        public bool IsPlayerTurn { get; set; }
        public bool IsGameStarted { get; set; }
        public Table PlayerTable { get; set; }
        public Table EnemyTable { get; set; }
        public List<Ship> Ships { get; set; }
        public byte TablesSize { get; set; }

        public int TotalShipPoints { get; set; }

        public int UserPoints { get; set; }
        public int EnemyPoints { get; set; }


        public InitGameModelDto(Table PlayerTable, Table EnemyTable, List<Ship> ships)
        {
            IsPlayerTurn = true;
            IsGameStarted = false;
            UserPoints = 0;
            EnemyPoints = 0;

            TablesSize = (byte)PlayerTable.Cells.GetLength(0);

            this.PlayerTable = PlayerTable;
            this.EnemyTable = EnemyTable;
            Ships = ships;

            foreach(var ship in Ships)
            {
                TotalShipPoints += ship.Lenght;
            }
        }
    }
}
