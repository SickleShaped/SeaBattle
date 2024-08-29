namespace SeaBattle.Models.Enums;

/// <summary>
/// Enum, определяющий, хранится ли в клетке корабль или нет
/// </summary>
public enum TilesType
{
    Free,
    Ship
}

/// <summary>
/// Enum, определяющий, "прострелена" ли клетка или нет
/// </summary>
public enum TilesVisibility
{
    Unchecked,
    Checked,
}