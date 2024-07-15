namespace SeaBattle.Models.AuxilaryModels;
/// <summary>
/// DTO всей игры
/// </summary>
/// <param name="playerTable"></param>
/// <param name="enemyTable"></param>
/// <param name="condition"></param>
public class FullGameDto(Table playerTable, Table enemyTable, GameCondition condition)
{
    public Table PlayerTable { get; set; } = playerTable;
    public Table EnemyTable { get; set; } = enemyTable;
    public GameCondition Condition { get; set; } = condition;
}
