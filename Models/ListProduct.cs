using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Grosu_Olesea_Lab7.Models
{
    public class ListProduct
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        // Relația cu ShopList
        [ForeignKey(typeof(ShopList))]
        public int ShopListID { get; set; }

        // Relația cu Product
        [ForeignKey(typeof(Product))]
        public int ProductID { get; set; }
    }
}

