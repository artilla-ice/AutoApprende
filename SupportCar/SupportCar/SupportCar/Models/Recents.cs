using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace SupportCar.Models
{
    [Table("Recents")]
    public class Recents
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public int IdItem { get; set; }
    }
}
