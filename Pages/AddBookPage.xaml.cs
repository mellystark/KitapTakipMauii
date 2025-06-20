using KitapTakipMauii.Models.Dtos;
using KitapTakipMauii.Services;

namespace KitapTakipMauii.Pages;

public partial class AddBookPage : ContentPage
{
    private readonly ApiService _apiService;

    public AddBookPage()
    {
        InitializeComponent();
        _apiService = new ApiService(new HttpClient()); // Gerekirse DI üzerinden de alabilirsin
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        var title = TitleEntry.Text;
        var author = AuthorEntry.Text;
        var genre = GenreEntry.Text;
        var description = DescriptionEntry.Text;
        var imageUrl = ImageUrlEntry.Text;
        var pageCountText = PageCountEntry.Text;

        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author))
        {
            await DisplayAlert("Hata", "Lütfen gerekli alanlarý doldurunuz.", "Tamam");
            return;
        }

        int? pageCount = null;
        if (!string.IsNullOrWhiteSpace(pageCountText) && int.TryParse(pageCountText, out var parsedPageCount))
        {
            pageCount = parsedPageCount;
        }
        else if (!string.IsNullOrWhiteSpace(pageCountText))
        {
            await DisplayAlert("Hata", "Sayfa sayýsý geçerli bir sayý olmalýdýr.", "Tamam");
            return;
        }

        var bookDto = new BookCreateDto
        {
            Title = title,
            Author = author,
            Genre = genre,
            Description = description,
            PageCount = pageCount,
            CoverImage = imageUrl
        };

        try
        {
            var response = await _apiService.AddBookAsync(bookDto);

            if (response.Success)
            {
                await DisplayAlert("Baþarýlý", "Kitap baþarýyla eklendi.", "Tamam");
                await Navigation.PopAsync(); // Önceki sayfaya dön
            }
            else
            {
                await DisplayAlert("Hata", response.Message ?? "Bir hata oluþtu", "Tamam");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Ýþlem sýrasýnda hata oluþtu:\n{ex.Message}", "Tamam");
        }
    }
}