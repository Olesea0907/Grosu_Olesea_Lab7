using Grosu_Olesea_Lab7.Models;
using Microsoft.Maui.ApplicationModel; // Corect pentru Map
using Microsoft.Maui.Devices.Sensors;

namespace Grosu_Olesea_Lab7;

public partial class ShopPage : ContentPage
{
    public ShopPage()
    {
        InitializeComponent();
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;

        if (shop == null || string.IsNullOrWhiteSpace(shop.ShopName) || string.IsNullOrWhiteSpace(shop.Adress))
        {
            await DisplayAlert("Error", "Please enter both Shop Name and Address.", "OK");
            return;
        }

        await App.Database.SaveShopAsync(shop);
        await Navigation.PopAsync();
    }

    private async void OnShowMapButtonClicked(object sender, EventArgs e)
    {
        try
        {
            var shop = (Shop)BindingContext;

            if (string.IsNullOrWhiteSpace(shop?.Adress))
            {
                await DisplayAlert("Error", "Address is required to show on map.", "OK");
                return;
            }

            var locations = await Geocoding.GetLocationsAsync(shop.Adress);
            var location = locations?.FirstOrDefault();

            if (location == null)
            {
                await DisplayAlert("Error", "Unable to locate the address.", "OK");
                return;
            }

            var options = new MapLaunchOptions
            {
                Name = shop.ShopName,
                NavigationMode = NavigationMode.Driving
            };

            await Microsoft.Maui.ApplicationModel.Map.OpenAsync(location, options);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }
}
