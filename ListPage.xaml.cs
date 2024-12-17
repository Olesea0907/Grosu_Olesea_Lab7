using Grosu_Olesea_Lab7.Models;

namespace Grosu_Olesea_Lab7;

public partial class ListPage : ContentPage
{
    public ListPage()
    {
        InitializeComponent();
    }

    /// Se apelează automat la afișarea paginii.
    /// Încarcă magazinele disponibile și adresele acestora.
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var items = await App.Database.GetShopsAsync();
        ShopPicker.ItemsSource = items;
        ShopPicker.ItemDisplayBinding = new Binding("ShopDetails");

        var shopList = (ShopList)BindingContext;

        if (shopList.ShopID != 0)
        {
            ShopPicker.SelectedItem = items.FirstOrDefault(s => s.ID == shopList.ShopID);
        }

        listView.ItemsSource = await App.Database.GetListProductsAsync(shopList.ID);
    }


    /// Salvează modificările aduse listei de cumpărături, inclusiv magazinul ales.
    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;

        slist.Date = DateTime.UtcNow;
        slist.Description = descriptionEditor.Text;

        // Salvează ID-ul magazinului selectat
        if (ShopPicker.SelectedItem is Shop selectedShop)
        {
            slist.ShopID = selectedShop.ID;
            slist.Shop = selectedShop; // Asigură-te că magazinul este setat
        }

        await App.Database.SaveShopListAsync(slist);
        await Navigation.PopAsync();
    }


    /// Șterge lista de cumpărături curentă.
    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        await App.Database.DeleteShopListAsync(slist);
        await Navigation.PopAsync();
    }

    /// Deschide pagina pentru alegerea unui produs.
    private async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductPage((ShopList)this.BindingContext)
        {
            BindingContext = new Product()
        });
    }

    /// Șterge un produs selectat din lista de cumpărături.
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

    /// Adaugă produsul selectat în Editor.
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

            ((ListView)sender).SelectedItem = null; // Resetează selecția
        }
    }
}
