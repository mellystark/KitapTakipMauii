using KitapTakipMauii.Pages;

namespace KitapTakipMauii;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
        Routing.RegisterRoute("BookAddPage", typeof(AddBookPage));
        Routing.RegisterRoute("MyBooksPage", typeof(Pages.MyBooksPage));
        Routing.RegisterRoute("BooksList", typeof(BooksPage));
        Routing.RegisterRoute("AdminPanel", typeof(Pages.AdminPanel));
    }
}