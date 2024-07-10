using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SeaBattle.Models.DbModels
{
    public class TablesDB
    {
        public Guid SessionId { get; set; }
        public Table UserTable { get; set; }
        public Table EnemyTable { get; set; }

    }
}
