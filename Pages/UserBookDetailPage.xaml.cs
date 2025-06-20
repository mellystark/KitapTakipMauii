using KitapTakipMauii.ViewModels;

namespace KitapTakipMauii.Pages;

[QueryProperty(nameof(BookId), "bookId")]
public partial class UserBookDetailPage : ContentPage
{
    public int BookId { get; set; }

    public UserBookDetailPage(UserBookDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is UserBookDetailViewModel viewModel)
        {
            await viewModel.LoadBookAsync();
        }
    }
}