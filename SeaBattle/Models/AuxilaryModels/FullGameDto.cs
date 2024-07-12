namespace SeaBattle.Models.AuxilaryModels;

public class FullGameDto(Table playerTable, Table enemyTable, GameCondition condition)
{
    public Table PlayerTable { get; set; } = playerTable;
    public Table EnemyTable { get; set; } = enemyTable;
    public GameCondition Condition { get; set; } = condition;
}
