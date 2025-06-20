using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KitapTakipMauii.Models.Dtos;
using KitapTakipMauii.Pages;
using KitapTakipMauii.Services;
using System.Collections.ObjectModel;

namespace KitapTakipMauii.ViewModels;

public partial class MyBooksPageViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    [ObservableProperty]
    private ObservableCollection<BookDto> books = new();

    [ObservableProperty]
    private string? selectedGenre;

    [ObservableProperty]
    private string? selectedAuthor;

    [ObservableProperty]
    private string? searchTitle;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private bool isNotBusy = true;

    public MyBooksPageViewModel(ApiService apiService)
    {
        _apiService = apiService;
        LoadUserBooksAsync();
    }

    [RelayCommand]
    private async Task NavigateToBookDetail(BookDto book)
    {
        if (book != null)
        {
            await Shell.Current.Navigation.PushAsync(new UserBookDetailPage(new UserBookDetailViewModel(_apiService, book.Id)));
        }
    }

    [RelayCommand]
    private async Task AddBook()
    {
        await Shell.Current.Navigation.PushAsync(new AddBookPage());
    }

    [RelayCommand]
    private async Task EditBook(BookDto book)
    {
        await Shell.Current.DisplayAlert("Bilgi", "Kitap düzenleme sayfası henüz uygulanmadı.", "Tamam");
    }

    [RelayCommand]
    private async Task DeleteBook(BookDto book)
    {
        if (book == null) return;

        var confirm = await Shell.Current.DisplayAlert("Onay", $"{book.Title} kitabını silmek istediğinizden emin misiniz?", "Evet", "Hayır");
        if (!confirm) return;

        IsBusy = true;
        try
        {
            var response = await _apiService.DeleteBookAsync(book.Id);
            if (response.Success)
            {
                Books.Remove(book);
                await Shell.Current.DisplayAlert("Başarılı", "Kitap silindi.", "Tamam");
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
        }
    }

    [RelayCommand]
    private async Task Logout()
    {
        try
        {
            await _apiService.ClearTokenAsync();
            await Shell.Current.GoToAsync("//LoginPage");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Hata", $"Çıkış yaparken hata oluştu: {ex.Message}", "Tamam");
        }
    }

    [RelayCommand]
    private async Task Refresh()
    {
        await LoadUserBooksAsync();
    }

    public async Task LoadUserBooksAsync()
    {
        IsBusy = true;
        IsNotBusy = false;

        try
        {
            var response = await _apiService.GetUserBooksAsync();
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
                await Shell.Current.DisplayAlert("Hata", response.Message, "Tamam");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Hata", $"Kullanıcı kitapları yüklenirken hata oluştu: {ex.Message}", "Tamam");
        }
        finally
        {
            IsBusy = false;
            IsNotBusy = true;
        }
    }

    public async Task FilterByGenreAsync()
    {
        if (string.IsNullOrEmpty(SelectedGenre))
        {
            await LoadUserBooksAsync();
        }
        else
        {
            IsBusy = true;
            IsNotBusy = false;

            try
            {
                var response = await _apiService.GetBooksByGenreAsync(SelectedGenre);
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
                    await Shell.Current.DisplayAlert("Hata", response.Message, "Tamam");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", $"Türe göre filtreleme sırasında hata oluştu: {ex.Message}", "Tamam");
            }
            finally
            {
                IsBusy = false;
                IsNotBusy = true;
            }
        }
    }

    public async Task FilterByAuthorAsync()
    {
        if (string.IsNullOrEmpty(SelectedAuthor))
        {
            await LoadUserBooksAsync();
        }
        else
        {
            IsBusy = true;
            IsNotBusy = false;

            try
            {
                var response = await _apiService.GetBooksByAuthorNameAsync(SelectedAuthor);
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
                    await Shell.Current.DisplayAlert("Hata", response.Message, "Tamam");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", $"Yazara göre filtreleme sırasında hata oluştu: {ex.Message}", "Tamam");
            }
            finally
            {
                IsBusy = false;
                IsNotBusy = true;
            }
        }
    }

    public async Task SearchByTitleAsync()
    {
        if (string.IsNullOrEmpty(SearchTitle))
        {
            await LoadUserBooksAsync();
        }
        else
        {
            IsBusy = true;
            IsNotBusy = false;

            try
            {
                var response = await _apiService.GetBooksByTitleAsync(SearchTitle);
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
                    await Shell.Current.DisplayAlert("Hata", response.Message, "Tamam");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", $"Başlık araması sırasında hata oluştu: {ex.Message}", "Tamam");
            }
            finally
            {
                IsBusy = false;
                IsNotBusy = true;
            }
        }
    }
}