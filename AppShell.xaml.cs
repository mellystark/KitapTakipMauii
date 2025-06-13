using KitapTakipMauii.Pages;

namespace KitapTakipMauii;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
        Routing.RegisterRoute("BookAddPage", typeof(BookAddPage));
    }
}