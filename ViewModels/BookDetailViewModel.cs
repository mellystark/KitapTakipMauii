using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

    [RelayCommand]
    private async Task ReadBookAsync()
    {
        IsBusy = true;
        IsNotBusy = false;

        try
        {
            // Prompt for reading details
            string startDateStr = await Application.Current.MainPage.DisplayPromptAsync(
                "Başlangıç Tarihi", "Okumaya başlayacağınız tarihi girin (dd/MM/yyyy):", keyboard: Keyboard.Text);
            if (startDateStr == null) // User canceled
                return;

            string endDateStr = await Application.Current.MainPage.DisplayPromptAsync(
                "Bitiş Tarihi", "Okumayı bitireceğiniz tarihi girin (dd/MM/yyyy):", keyboard: Keyboard.Text);
            if (endDateStr == null)
                return;

            string notes = await Application.Current.MainPage.DisplayPromptAsync(
                "Notlar", "Kitap hakkında notlarınızı girin:", maxLength: 500, keyboard: Keyboard.Text);
            if (notes == null)
                return;

            // Parse dates
            if (!DateTime.TryParseExact(startDateStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime startDate))
            {
                await Application.Current.MainPage.DisplayAlert("Hata", "Geçersiz başlangıç tarihi formatı.", "Tamam");
                return;
            }

            if (!DateTime.TryParseExact(endDateStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime endDate))
            {
                await Application.Current.MainPage.DisplayAlert("Hata", "Geçersiz bitiş tarihi formatı.", "Tamam");
                return;
            }

            // Validate EndDate is not before StartDate
            if (endDate < startDate)
            {
                await Application.Current.MainPage.DisplayAlert("Hata", "Bitiş tarihi başlangıç tarihinden önce olamaz.", "Tamam");
                return;
            }

            // Create BookUpdateDto with current book details and new reading details
            var bookUpdateDto = new BookUpdateDto
            {
                Id = Book.Id,
                Title = Book.Title,
                Author = Book.Author,
                Genre = Book.Genre,
                Notes = notes,
                Description = Book.Description,
                PageCount = Book.PageCount,
                CoverImage = Book.CoverImage,
                StartDate = startDate,
                EndDate = endDate,
                IsReading = true
            };

            var response = await _apiService.UpdateBookAsync(_bookId, bookUpdateDto);
            if (response.Success && response.Data != null)
            {
                Book = response.Data; // Update UI
                await Application.Current.MainPage.DisplayAlert("Başarılı", "Okuma detayları kaydedildi.", "Tamam");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Hata", response.Message, "Tamam");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Hata", $"Okuma detayları kaydedilirken hata oluştu: {ex.Message}", "Tamam");
        }
        finally
        {
            IsBusy = false;
            IsNotBusy = true;
        }
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