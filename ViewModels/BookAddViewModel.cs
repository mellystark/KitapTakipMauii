using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KitapTakipMauii.Models.Dtos;
using KitapTakipMauii.Services;

namespace KitapTakipMauii.ViewModels;

[QueryProperty(nameof(Book), "Book")]
public partial class BookAddViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    [ObservableProperty]
    private int id;

    [ObservableProperty]
    private string title;

    [ObservableProperty]
    private string author;

    [ObservableProperty]
    private string genre;

    [ObservableProperty]
    private string notes;

    [ObservableProperty]
    private DateTime startDate = DateTime.Today;

    [ObservableProperty]
    private DateTime endDate = DateTime.Today;

    [ObservableProperty]
    private BookDto book;

    private bool _isEditing;

    public BookAddViewModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    partial void OnBookChanged(BookDto value)
    {
        if (value != null)
        {
            Id = value.Id;
            Title = value.Title;
            Author = value.Author;
            Genre = value.Genre;
            Notes = value.Notes;
            StartDate = value.StartDate ?? DateTime.Today;
            EndDate = value.EndDate ?? DateTime.Today;
            _isEditing = true;
        }
    }

    [RelayCommand]
    private async Task SaveBookAsync()
    {
        if (_isEditing)
        {
            var bookUpdateDto = new BookUpdateDto
            {
                Id = Id,
                Title = Title,
                Author = Author,
                Genre = Genre,
                Notes = Notes
            };
            var response = await _apiService.UpdateBookAsync(Id, bookUpdateDto);
            if (response.Success)
            {
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Hata", response.Message, "Tamam");
            }
        }
        else
        {
            var bookCreateDto = new BookCreateDto
            {
                Title = Title,
                Author = Author,
                Genre = Genre,
                Notes = Notes
            };
            var response = await _apiService.AddBookAsync(bookCreateDto);
            if (response.Success)
            {
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Hata", response.Message, "Tamam");
            }
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}