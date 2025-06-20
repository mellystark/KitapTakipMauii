using KitapTakipMauii.ViewModels;

namespace KitapTakipMauii.Pages;

public partial class MyBooksPage : ContentPage
{
    private readonly MyBooksPageViewModel _viewModel;

    public MyBooksPage(MyBooksPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is MyBooksPageViewModel viewModel)
        {
            await viewModel.LoadUserBooksAsync();
        }
    }

    private async void GenreEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        await _viewModel.FilterByGenreAsync();
    }

    private async void AuthorEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        await _viewModel.FilterByAuthorAsync();
    }

    private async void SearchTitle_TextChanged(object sender, TextChangedEventArgs e)
    {
        await _viewModel.SearchByTitleAsync();
    }
}