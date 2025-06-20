using KitapTakipMauii.ViewModels;

namespace KitapTakipMauii.Pages;

public partial class AdminPanel : ContentPage
{
    public AdminPanel(AdminPanelViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
