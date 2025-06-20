using KitapTakipMauii.ViewModels;

namespace KitapTakipMauii.Pages;

public partial class BookDetailPage : ContentPage
{
    public BookDetailPage(BookDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}