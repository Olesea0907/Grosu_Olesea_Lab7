using Microsoft.Maui.Controls;

namespace Grosu_Olesea_Lab7;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("MainPage", typeof(MainPage));
        Routing.RegisterRoute("AboutPage", typeof(AboutPage));
        Routing.RegisterRoute("ListEntryPage", typeof(ListEntryPage));
        Routing.RegisterRoute("ShopEntryPage", typeof(ShopEntryPage));
    }
}
