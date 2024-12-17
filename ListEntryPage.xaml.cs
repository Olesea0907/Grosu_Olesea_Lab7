using Grosu_Olesea_Lab7.Models;
using System;
using System.Threading.Tasks;

namespace Grosu_Olesea_Lab7
{
    public partial class ListEntryPage : ContentPage
    {
        public ListEntryPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var shopLists = await App.Database.GetShopListsAsync();
            foreach (var sl in shopLists)
            {
                System.Diagnostics.Debug.WriteLine($"ShopList ID: {sl.ID}, Description: {sl.Description}, ShopID: {sl.ShopID}");
            }
            listView.ItemsSource = shopLists;
        }


        private async void OnShopListAddedClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListPage
            {
                BindingContext = new ShopList()
            });
        }

        private async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new ListPage
                {
                    BindingContext = e.SelectedItem as ShopList
                });
            }
        }
    }
}
