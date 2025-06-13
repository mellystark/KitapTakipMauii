using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KitapTakipMauii.Models.Dtos;
using KitapTakipMauii.Models.Responses;
using KitapTakipMauii.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace KitapTakipMauii.ViewModels;

public partial class BooksViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    [ObservableProperty]
    private ObservableCollection<BookDto> books;

    [ObservableProperty]
    private ObservableCollection<string> genres;

    [ObservableProperty]
    private string selectedGenre;

    [ObservableProperty]
    private ObservableCollection<string> authors;

    [ObservableProperty]
    private string selectedAuthor;

    [ObservableProperty]
    private string searchTitle;

    [ObservableProperty]
    private bool isBusy;

    public BooksViewModel(ApiService apiService)
    {
        _apiService = apiService;
        Books = new ObservableCollection<BookDto>();
        Genres = new ObservableCollection<string>();
        Authors = new ObservableCollection<string>();
        // Constructor'da asenkron işlem başlatıyoruz
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        await LoadBooksAsync();
    }

    private async Task LoadBooksAsync()
    {
        try
        {
            IsBusy = true;
            Debug.WriteLine("Kitaplar yükleniyor...");
            var response = await _apiService.GetBooksAsync();
            if (response.Success)
            {
                Books.Clear();
                Genres.Clear();
                Authors.Clear();
                foreach (var book in response.Data)
                {
                    Books.Add(book);
                    if (!Genres.Contains(book.Genre) && !string.IsNullOrEmpty(book.Genre))
                        Genres.Add(book.Genre);
                    if (!Authors.Contains(book.Author) && !string.IsNullOrEmpty(book.Author))
                        Authors.Add(book.Author);
                }
                Debug.WriteLine($"Kitaplar yüklendi: {Books.Count} adet");
            }
            else
            {
                await Shell.Current.DisplayAlert("Hata", response.Message, "Tamam");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Kitap yükleme hatası: {ex.Message}, StackTrace: {ex.StackTrace}");
            await Shell.Current.DisplayAlert("Hata", "Kitaplar yüklenirken bir hata oluştu.", "Tamam");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task FilterByGenreAsync()
    {
        try
        {
            IsBusy = true;
            if (string.IsNullOrEmpty(SelectedGenre))
            {
                await LoadBooksAsync();
            }
            else
            {
                var response = await _apiService.GetBooksByGenreAsync(SelectedGenre);
                if (response.Success)
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
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Tür filtreleme hatası: {ex.Message}");
            await Shell.Current.DisplayAlert("Hata", "Tür filtresi uygulanırken bir hata oluştu.", "Tamam");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task FilterByAuthorAsync()
    {
        try
        {
            IsBusy = true;
            if (string.IsNullOrEmpty(SelectedAuthor))
            {
                await LoadBooksAsync();
            }
            else
            {
                var response = await _apiService.GetBooksByAuthorNameAsync(SelectedAuthor);
                if (response.Success)
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
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Yazar filtreleme hatası: {ex.Message}");
            await Shell.Current.DisplayAlert("Hata", "Yazar filtresi uygulanırken bir hata oluştu.", "Tamam");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task FilterByTitleAsync()
    {
        try
        {
            IsBusy = true;
            if (string.IsNullOrEmpty(SearchTitle))
            {
                await LoadBooksAsync();
            }
            else
            {
                var response = await _apiService.GetBooksByTitleAsync(SearchTitle);
                if (response.Success)
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
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Başlık filtreleme hatası: {ex.Message}");
            await Shell.Current.DisplayAlert("Hata", "Başlık aranırken bir hata oluştu.", "Tamam");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task AddBookAsync()
    {
        await Shell.Current.GoToAsync("BookAddPage");
    }

    [RelayCommand]
    public async Task EditBookAsync(BookDto bookDto)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { "Book", bookDto }
        };
        await Shell.Current.GoToAsync("BookAddPage", true, navigationParameter);
    }

    [RelayCommand]
    public async Task DeleteBookAsync(BookDto bookDto)
    {
        var confirm = await Shell.Current.DisplayAlert("Sil", $"{bookDto.Title} adlı kitabı silmek istediğinizden emin misiniz?", "Evet", "Hayır");
        if (confirm)
        {
            var response = await _apiService.DeleteBookAsync(bookDto.Id);
            if (response.Success)
            {
                Books.Remove(bookDto);
            }
            else
            {
                await Shell.Current.DisplayAlert("Hata", response.Message, "Tamam");
            }
        }
    }

    [RelayCommand]
    public async Task LogoutAsync()
    {
        await _apiService.ClearTokenAsync();
        await Shell.Current.GoToAsync("//LoginPage");
    }
}