using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SeaBattle.Consts;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;
using System.Drawing;

namespace SeaBattle.Models;

/// <summary>
/// Класс, представляющий собой таблицу
/// </summary>
public class Table
{
    public TilesType[,] Cells { get; set; }
    public TilesVisibility[,] CellsVisibility { get; set; }
    public byte CurrentScore {  get; set; } 


    public Table()
    {
        Cells = new TilesType[Constants.TableWidth, Constants.TableWidth];
        CellsVisibility = new TilesVisibility[Constants.TableWidth, Constants.TableWidth];
    }
}
