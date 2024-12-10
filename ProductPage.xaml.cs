using System;
using System.Threading.Tasks;
using Grosu_Olesea_Lab7.Models;

namespace Grosu_Olesea_Lab7
{
    public partial class ProductPage : ContentPage
    {
        public ProductPage()
        {
            InitializeComponent();
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
    }
}
