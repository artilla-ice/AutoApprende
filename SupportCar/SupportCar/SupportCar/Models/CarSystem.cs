using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SupportCar.Models
{
    [Table("CarSystem")]
    public class CarSystem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string FolderName { get; set; }
    }
}
