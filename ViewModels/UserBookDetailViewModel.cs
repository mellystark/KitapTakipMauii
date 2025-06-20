using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KitapTakipMauii.Models.Dtos;
using KitapTakipMauii.Services;
using KitapTakipMauii.Pages;

namespace KitapTakipMauii.ViewModels;

public partial class UserBookDetailViewModel : ObservableObject
{
    private readonly ApiService _apiService;
    private readonly int _bookId;

    [ObservableProperty]
    private BookDto book = new();

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private bool isNotBusy = true;

    public UserBookDetailViewModel(ApiService apiService, int bookId)
    {
        _apiService = apiService;
        _bookId = bookId;
    }

    public async Task LoadBookAsync()
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
                await Shell.Current.DisplayAlert("Hata", response.Message ?? "Kitap yüklenemedi.", "Tamam");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Hata", $"Kitap yüklenirken hata oluştu: {ex.Message}", "Tamam");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task EditBookAsync()
    {
        await Shell.Current.Navigation.PushAsync(new EditBookPage(new EditBookViewModel(_apiService, _bookId)));
    }

    [RelayCommand]
    private async Task DeleteBookAsync()
    {
        if (Book == null) return;

        var confirm = await Shell.Current.DisplayAlert("Onay", $"{Book.Title} kitabını silmek istediğinizden emin misiniz?", "Evet", "Hayır");
        if (!confirm) return;

        IsBusy = true;
        try
        {
            var response = await _apiService.DeleteBookAsync(Book.Id);
            if (response.Success)
            {
                await Shell.Current.DisplayAlert("Başarılı", "Kitap silindi.", "Tamam");
                await Shell.Current.Navigation.PopAsync();
            }
            else
            {
                await Shell.Current.DisplayAlert("Hata", response.Message, "Tamam");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Hata", $"Kitap silinirken hata oluştu: {ex.Message}", "Tamam");
        }
        finally
        {
            IsBusy = false;
            IsNotBusy = true;
        }
    }
}