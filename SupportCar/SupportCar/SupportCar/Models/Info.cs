using System;
using System.Collections.Generic;
using System.Text;

using SQLite;


namespace SupportCar.Models
{
    [Table("Info")]
    public class Info
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string FolderPath { get; set; }

    }
}
