using KitapTakipMauii.ViewModels;

namespace KitapTakipMauii.Pages;

public partial class ReadBooksPage : ContentPage
{
    private readonly ReadBooksViewModel _viewModel;

    public ReadBooksPage(ReadBooksViewModel viewModel)
    {
        _viewModel = viewModel; // Önce atama
        InitializeComponent();
        BindingContext = _viewModel; // Sonra baðlama
    }

    private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        bool canExecute = _viewModel?.SearchBooksCommand?.CanExecute(null) ?? false;
        if (_viewModel != null && canExecute)
        {
            _viewModel.SearchBooksCommand.Execute(null);
        }
        else
        {
            // Hata ayýklama için konsola mesaj yaz
            System.Diagnostics.Debug.WriteLine("SearchBooksCommand is null or cannot execute.");
        }
    }
}