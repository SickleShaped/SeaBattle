using SeaBattle.Models.Enums;

namespace SeaBattle.Models
{
    /// <summary>
    /// Класс, представляющий собой таблицу
    /// </summary>
    public class Table
    {
        /// <summary>
        /// Значение полей таблицы
        /// </summary>
        public TilesType[,] Cells { get; set; }

        public TilesVisibility[,] CellsVisibility { get; set; }

        /// <summary>
        /// Принадлежит ли поле игроку. True - если да, False - если принадлежит боту/сопернику
        /// </summary>
        public bool BelongsPlayer { get; set; }
        public Table(bool belongsPlayer)
        {
            BelongsPlayer = belongsPlayer;
            Cells = new TilesType[10, 10];
            CellsVisibility = new TilesVisibility[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Cells[i, j] = TilesType.Free;
                    if (BelongsPlayer)
                    {
                        CellsVisibility[i, j] = TilesVisibility.Checked;
                    }
                    else
                    {
                        CellsVisibility[i, j] = TilesVisibility.Unchecked;
                    }
                }
            }
        }



    }
}
