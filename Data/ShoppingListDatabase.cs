using System;
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
        }

        public Task<List<ShopList>> GetShopListsAsync()
        {
            return _database.Table<ShopList>().ToListAsync();
        }

        public Task<ShopList> GetShopListAsync(int id)
        {
            return _database.Table<ShopList>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveShopListAsync(ShopList slist)
        {
            if (slist.ID != 0)
            {
                return _database.UpdateAsync(slist);
            }
            else
            {
                return _database.InsertAsync(slist);
            }
        }

        public Task<int> DeleteShopListAsync(ShopList slist)
        {
            return _database.DeleteAsync(slist);
        }

        public Task<int> SaveProductAsync(Product product)
        {
            if (product.ID != 0)
            {
                return _database.UpdateAsync(product);
            }
            else
            {
                return _database.InsertAsync(product);
            }
        }

        public Task<int> DeleteProductAsync(Product product)
        {
            return _database.DeleteAsync(product);
        }

        public Task<List<Product>> GetProductsAsync()
        {
            return _database.Table<Product>().ToListAsync();
        }

        public async Task<int> SaveListProductAsync(ListProduct listProduct)
        {
            var existingProduct = await GetListProductAsync(listProduct.ShopListID, listProduct.ProductID);
            if (existingProduct != null)
            {
                System.Diagnostics.Debug.WriteLine($"Product ID {listProduct.ProductID} already exists in ShopList ID {listProduct.ShopListID}.");
                return 0; 
            }

            return await _database.InsertAsync(listProduct);
        }


        public Task<ListProduct> GetListProductAsync(int shopListID, int productID)
        {
            return _database.Table<ListProduct>()
                            .Where(lp => lp.ShopListID == shopListID && lp.ProductID == productID)
                            .FirstOrDefaultAsync();
        }

        public Task<int> DeleteListProductAsync(ListProduct listProduct)
        {
            return _database.DeleteAsync(listProduct);
        }
        public async Task<int> DeleteListProductAsync(int productId, int shopListId)
{
    return await _database.ExecuteAsync(
        "DELETE FROM ListProduct WHERE ProductID = ? AND ShopListID = ?", 
        productId, 
        shopListId);
}



        public async Task<List<Product>> GetListProductsAsync(int shoplistid)
        {
            var result = await _database.QueryAsync<Product>(
                "SELECT P.ID, P.Description FROM Product P " +
                "INNER JOIN ListProduct LP ON P.ID = LP.ProductID " +
                "WHERE LP.ShopListID = ?",
                shoplistid);

            foreach (var product in result)
            {
                System.Diagnostics.Debug.WriteLine($"Product ID: {product.ID}, Description: {product.Description}");
            }

            return result;
        }
    }
}
