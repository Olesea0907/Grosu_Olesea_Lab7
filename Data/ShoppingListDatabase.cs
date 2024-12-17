using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Grosu_Olesea_Lab7.Models;

namespace Grosu_Olesea_Lab7.Data
{
    public class ShoppingListDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public ShoppingListDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<ShopList>().Wait();
            _database.CreateTableAsync<Product>().Wait();
            _database.CreateTableAsync<ListProduct>().Wait();
            _database.CreateTableAsync<Shop>().Wait();
        }

        // ShopList
        public async Task<List<ShopList>> GetShopListsAsync()
        {
            // Obține lista de cumpărături
            var shopLists = await _database.Table<ShopList>().ToListAsync();

            // Încarcă magazinul asociat pentru fiecare listă
            foreach (var list in shopLists)
            {
                if (list.ShopID != 0)
                {
                    list.Shop = await _database.Table<Shop>()
                                               .Where(s => s.ID == list.ShopID)
                                               .FirstOrDefaultAsync();
                }
            }

            return shopLists;
        }
        public Task<int> SaveShopListAsync(ShopList shopList) => shopList.ID != 0 ? _database.UpdateAsync(shopList) : _database.InsertAsync(shopList);
        public Task<int> DeleteShopListAsync(ShopList shopList) => _database.DeleteAsync(shopList);

        // Product
        public Task<List<Product>> GetProductsAsync() => _database.Table<Product>().ToListAsync();
        public Task<int> SaveProductAsync(Product product) => product.ID != 0 ? _database.UpdateAsync(product) : _database.InsertAsync(product);
        public Task<int> DeleteProductAsync(Product product) => _database.DeleteAsync(product);

        // ListProduct
        public Task<int> SaveListProductAsync(ListProduct listProduct) => _database.InsertAsync(listProduct);
        public Task<int> DeleteListProductAsync(int productId, int shopListId)
        {
            return _database.ExecuteAsync("DELETE FROM ListProduct WHERE ProductID = ? AND ShopListID = ?", productId, shopListId);
        }
        public Task<List<Product>> GetListProductsAsync(int shopListID)
        {
            return _database.QueryAsync<Product>(
                "SELECT P.* FROM Product P " +
                "INNER JOIN ListProduct LP ON P.ID = LP.ProductID " +
                "WHERE LP.ShopListID = ?", shopListID);
        }

        public Task<ListProduct> GetListProductAsync(int shopListId, int productId)
        {
            return _database.Table<ListProduct>()
                            .Where(lp => lp.ShopListID == shopListId && lp.ProductID == productId)
                            .FirstOrDefaultAsync();
        }

        // Shop
        public Task<List<Shop>> GetShopsAsync() => _database.Table<Shop>().ToListAsync();
        public Task<int> SaveShopAsync(Shop shop) => shop.ID != 0 ? _database.UpdateAsync(shop) : _database.InsertAsync(shop);
        public Task<int> DeleteShopAsync(Shop shop) => _database.DeleteAsync(shop);
    }
}
