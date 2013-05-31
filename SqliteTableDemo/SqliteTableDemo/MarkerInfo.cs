using System;
using SQLite;

namespace SqliteTableDemo {
    public class MarkerInfo {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}