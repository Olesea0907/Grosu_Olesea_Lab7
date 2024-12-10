using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace Grosu_Olesea_Lab7.Models
{
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Description { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<ListProduct> ListProducts { get; set; }
    }
}
