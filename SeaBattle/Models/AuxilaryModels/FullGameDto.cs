using SeaBattle.Models.DbModels;
using SeaBattle.Models.Tables;

namespace SeaBattle.Models.AuxilaryModels
{
    public class FullGameDto
    {
        public Table PlayerTable { get; set; }
        public Table EnemyTable { get; set; }
        public GameCondition Condition { get; set; }


        public FullGameDto() { }

        public FullGameDto(Table playerTable, Table enemyTable, GameCondition condition)
        {
            PlayerTable = playerTable;
            EnemyTable = enemyTable;
            Condition = condition;
        }
    }
}
