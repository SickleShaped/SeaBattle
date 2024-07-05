using SeaBattle.Models.Enums;

namespace SeaBattle.Models.Tables
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

        /// <summary>
        /// Длина или высота таблицы
        /// </summary>
        public byte Size { get; set; }

        /// <summary>
        /// Принадлежит ли поле игроку. True - если да, False - если принадлежит боту/сопернику
        /// </summary>
        public bool BelongsPlayer { get; set; }
        public Table(byte size, bool belongsPlayer)
        {
            Cells = new TilesType[size,size];
            Size = size;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Cells[i,j]=TilesType.FreeUnchecked;
                }
            }
        }
    }
}
