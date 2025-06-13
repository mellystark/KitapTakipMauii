using KitapTakipMauii.ViewModels;

namespace KitapTakipMauii.Pages;

public partial class BookAddPage : ContentPage
{
    public BookAddPage(BookAddViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}