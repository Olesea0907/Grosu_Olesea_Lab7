using Grosu_Olesea_Lab7.Data;
using System;
using System.IO;

namespace Grosu_Olesea_Lab7
{
    public partial class App : Application
    {
        private static ShoppingListDatabase database = null!;

        public static ShoppingListDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ShoppingListDatabase(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ShoppingList.db3"));
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
