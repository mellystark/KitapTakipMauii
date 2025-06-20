using CommunityToolkit.Mvvm.ComponentModel;
using KitapTakipMauii.Models.Dtos;
using KitapTakipMauii.Services;

namespace KitapTakipMauii.ViewModels;

public partial class BookDetailViewModel : ObservableObject
{
    private readonly ApiService _apiService;
    private readonly int _bookId;

    [ObservableProperty]
    private BookDto book = new();

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private bool isNotBusy = true;

    public BookDetailViewModel(ApiService apiService, int bookId)
    {
        _apiService = apiService;
        _bookId = bookId;
        LoadBookDetailsAsync();
    }

    private async Task LoadBookDetailsAsync()
    {
        IsBusy = true;
        IsNotBusy = false;

        try
        {
            var response = await _apiService.GetBookByIdAsync(_bookId);
            if (response.Success && response.Data != null)
            {
                Book = response.Data;
            }
            else
            {
                await Shell.Current.DisplayAlert("Hata", response.Message, "Tamam");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Hata", $"Kitap detayları yüklenirken hata oluştu: {ex.Message}", "Tamam");
        }
        finally
        {
            IsBusy = false;
            IsNotBusy = true;
        }
    }
}