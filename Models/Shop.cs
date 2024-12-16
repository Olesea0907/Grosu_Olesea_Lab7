using SQLite; 
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace Grosu_Olesea_Lab7.Models
{
    public class Shop
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string ShopName { get; set; }

        // Adresa magazinului
        public string Adress { get; set; }

        public string ShopDetails
        {
            get { return ShopName + "\n" + Adress; }
        }

        [OneToMany]
        public List<ShopList> ShopLists { get; set; }
    }
}
