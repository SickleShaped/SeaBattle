namespace SeaBattle.Models.AuxilaryModels;

public class Game
{
    public Table PlayerTable { get; set; }
    public Table EnemyTable { get; set; }
    public GameCondition Condition { get; set; }
}
