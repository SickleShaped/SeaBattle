using SeaBattle.Models.Tables;
using System.Security.Cryptography.X509Certificates;

namespace SeaBattle.Models.AuxilaryModels
{
    public class InitGameModelDto
    {
        public bool IsPlayerTurn { get; set; }
        public bool IsGameStarted { get; set; }
        public Table PlayerTable { get; set; }
        public Table EnemyTable { get; set; }
        public List<Ship> Ships { get; set; }
        public byte TablesSize { get; set; }


        public InitGameModelDto(Table PlayerTable, Table EnemyTable, List<Ship> ships)
        {
            IsPlayerTurn = true;
            IsGameStarted = false;

            TablesSize = (byte)PlayerTable.Cells.GetLength(0);

            this.PlayerTable = PlayerTable;
            this.EnemyTable = EnemyTable;
            Ships = ships;
        }
    }
}
