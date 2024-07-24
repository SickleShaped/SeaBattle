using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;
using SeaBattle.Services.Implementations;

namespace SeaBattle.Models;
/// <summary>
/// Класс, содержащий поля текущего состояния игры
/// </summary>
public class GameCondition
{
    public GameState GameState { get; set; }
    public LastRequestResult LastRequestResult { get; set; }
    public List<Ship> Ships { get; set; }
    public Coordinate lastBotShoot { get; set; }
    public GameCondition(List<Ship> ships) => Ships = ships;
    
}
