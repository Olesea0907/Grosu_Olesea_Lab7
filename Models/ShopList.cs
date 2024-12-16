using SQLite; 
using SQLiteNetExtensions.Attributes; 
using System;

namespace Grosu_Olesea_Lab7.Models
{
    public class ShopList
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [MaxLength(250), Unique]
        public string Description { get; set; }

        public DateTime Date { get; set; }

        [ForeignKey(typeof(Shop))]
        public int ShopID { get; set; }

        [ManyToOne]
        public Shop Shop { get; set; }
    }
}
