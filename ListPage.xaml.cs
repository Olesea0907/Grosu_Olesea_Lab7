using System;
using Grosu_Olesea_Lab7.Models;

namespace Grosu_Olesea_Lab7
{
    public partial class ListPage : ContentPage
    {
        public ListPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var shopList = (ShopList)BindingContext;

            System.Diagnostics.Debug.WriteLine($"ShopList ID: {shopList.ID}, Description: {shopList.Description}");

            listView.ItemsSource = await App.Database.GetListProductsAsync(shopList.ID);
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var slist = (ShopList)BindingContext;
            slist.Date = DateTime.UtcNow;
            slist.Description = descriptionEditor.Text; 
            await App.Database.SaveShopListAsync(slist);
            await Navigation.PopAsync();
        }

        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var slist = (ShopList)BindingContext;
            await App.Database.DeleteShopListAsync(slist);
            await Navigation.PopAsync();
        }

        private async void OnChooseButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductPage((ShopList)this.BindingContext)
            {
                BindingContext = new Product()
            });
        }

        private async void OnDeleteItemClicked(object sender, EventArgs e)
        {
            var shopList = (ShopList)BindingContext;

            var products = await App.Database.GetListProductsAsync(shopList.ID);
            if (products.Count == 0)
            {
                await DisplayAlert("Warning", "No items to delete.", "OK");
                return;
            }

            string[] productNames = products.Select(p => p.Description).ToArray();
            string selectedProductName = await DisplayActionSheet("Select item to delete", "Cancel", null, productNames);

            if (selectedProductName != null && selectedProductName != "Cancel")
            {
                var productToDelete = products.FirstOrDefault(p => p.Description == selectedProductName);
                if (productToDelete != null)
                {
                    await App.Database.DeleteListProductAsync(productToDelete.ID, shopList.ID);

                    listView.ItemsSource = await App.Database.GetListProductsAsync(shopList.ID);
                    await DisplayAlert("Success", $"{productToDelete.Description} deleted.", "OK");
                }
            }
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedProduct = e.SelectedItem as Product;
            if (selectedProduct != null)
            {
                if (!string.IsNullOrEmpty(descriptionEditor.Text))
                {
                    if (!descriptionEditor.Text.Contains(selectedProduct.Description))
                    {
                        descriptionEditor.Text += $"{selectedProduct.Description}, ";
                    }
                }
                else
                {
                    descriptionEditor.Text = $"{selectedProduct.Description}, ";
                }

                listView.SelectedItem = null;
            }
        }
        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Product selectedProduct)
            {
                if (!string.IsNullOrEmpty(descriptionEditor.Text))
                {
                    if (!descriptionEditor.Text.Contains(selectedProduct.Description))
                    {
                        descriptionEditor.Text += $"{selectedProduct.Description}, ";
                    }
                }
                else
                {
                    descriptionEditor.Text = $"{selectedProduct.Description}, ";
                }

                ((ListView)sender).SelectedItem = null;
            }
        }

    }

}
