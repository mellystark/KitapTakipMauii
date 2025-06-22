using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KitapTakipMauii.Models.Dtos;
using KitapTakipMauii.Services;
using System.Collections.ObjectModel;

namespace KitapTakipMauii.ViewModels;

public partial class ReadBooksViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    [ObservableProperty]
    private ObservableCollection<BookDto> books = new();

    [ObservableProperty]
    private BookDto? selectedBook;

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    public ReadBooksViewModel(ApiService apiService)
    {
        _apiService = apiService;
        LoadReadBooksAsync();
    }

    public async Task<string> GetUsernameAsync()
    {
        return await SecureStorage.GetAsync("username") ?? string.Empty;
    }

    [RelayCommand]
    public async Task LoadReadBooksAsync()
    {
        IsBusy = true;

        try
        {
            var username = await GetUsernameAsync();
            if (string.IsNullOrEmpty(username))
            {
                await Shell.Current.DisplayAlert("Hata", "Kullanıcı oturumu bulunamadı.", "Tamam");
                return;
            }

            var response = await _apiService.GetReadBooksAsync(username);
            if (response.Success && response.Data != null)
            {
                Books.Clear();
                foreach (var book in response.Data)
                {
                    Books.Add(book);
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Hata", response.Message ?? "Kitaplar yüklenemedi.", "Tamam");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Hata", $"Kitaplar yüklenirken hata oluştu: {ex.Message}", "Tamam");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task SearchBooksAsync()
    {
        IsBusy = true;

        try
        {
            var username = await GetUsernameAsync();
            if (string.IsNullOrEmpty(username))
            {
                await Shell.Current.DisplayAlert("Hata", "Kullanıcı oturumu bulunamadı.", "Tamam");
                return;
            }

            var response = await _apiService.GetReadBooksAsync(username, SearchText);
            if (response.Success && response.Data != null)
            {
                Books.Clear();
                foreach (var book in response.Data)
                {
                    Books.Add(book);
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Hata", response.Message ?? "Kitaplar aranamadı.", "Tamam");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Hata", $"Kitaplar aranırken hata oluştu: {ex.Message}", "Tamam");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task BookSelectedAsync()
    {
        if (SelectedBook == null)
            return;

        // Show details in a popup
        await Shell.Current.DisplayAlert(
            SelectedBook.Title,
            $"Başlama Tarihi: {SelectedBook.StartDate?.ToString("dd.MM.yyyy") ?? "Belirtilmemiş"}\n" +
            $"Bitiş Tarihi: {SelectedBook.EndDate?.ToString("dd.MM.yyyy") ?? "Belirtilmemiş"}\n" +
            $"Notlar: {SelectedBook.Notes ?? "Yok"}",
            "Tamam");

        SelectedBook = null; // Clear selection after showing details
    }
}