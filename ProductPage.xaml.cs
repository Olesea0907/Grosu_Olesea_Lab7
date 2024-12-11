using System;
using System.Threading.Tasks;
using Grosu_Olesea_Lab7.Models;

namespace Grosu_Olesea_Lab7
{
    public partial class ProductPage : ContentPage
    {
        private ShopList _shopList;

        public ProductPage(ShopList shopList)
        {
            InitializeComponent();
            _shopList = shopList;
            BindingContext = new Product();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            listView.ItemsSource = await App.Database.GetProductsAsync();
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var product = (Product)BindingContext;
            if (!string.IsNullOrWhiteSpace(product.Description))
            {
                await App.Database.SaveProductAsync(product);
                listView.ItemsSource = await App.Database.GetProductsAsync();
                BindingContext = new Product();
            }
            else
            {
                await DisplayAlert("Warning", "Product name cannot be empty.", "OK");
            }
        }

        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var product = listView.SelectedItem as Product;
            if (product != null)
            {
                await App.Database.DeleteProductAsync(product);
                listView.ItemsSource = await App.Database.GetProductsAsync();
            }
            else
            {
                await DisplayAlert("Warning", "No product selected.", "OK");
            }
        }

        private async void OnAddButtonClicked(object sender, EventArgs e)
        {
            Product selectedProduct = listView.SelectedItem as Product;

            if (selectedProduct != null)
            {
                var existingProduct = await App.Database.GetListProductAsync(_shopList.ID, selectedProduct.ID);
                if (existingProduct != null)
                {
                    await DisplayAlert("Info", $"{selectedProduct.Description} is already in the shopping list.", "OK");
                    return;
                }

                var listProduct = new ListProduct
                {
                    ShopListID = _shopList.ID,
                    ProductID = selectedProduct.ID
                };

                await App.Database.SaveListProductAsync(listProduct);

                selectedProduct.ListProducts = new System.Collections.Generic.List<ListProduct> { listProduct };

                await DisplayAlert("Success", $"{selectedProduct.Description} added to the shopping list.", "OK");
                await Navigation.PopAsync(); 
            }
            else
            {
                await DisplayAlert("Warning", "No product selected.", "OK");
            }
        }
    }
}
