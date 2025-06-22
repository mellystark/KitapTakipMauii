using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KitapTakipMauii.Models.Responses;
using KitapTakipMauii.Services;
using System.Collections.ObjectModel;

namespace KitapTakipMauii.ViewModels;

public partial class AdminPanelViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    public ObservableCollection<ListItemViewModel> UserItems { get; } = new();
    public ObservableCollection<ListItemViewModel> BookItems { get; } = new();

    public ObservableCollection<ListItemViewModel> CurrentItems => IsUsersSelected ? UserItems : BookItems;

    [ObservableProperty]
    private bool isUsersSelected;

    [ObservableProperty]
    private bool isBooksSelected;

    public AdminPanelViewModel(ApiService apiService)
    {
        _apiService = apiService;

        IsUsersSelected = true;
        IsBooksSelected = false;

        LoadUsersAsync();

        PropertyChanged += async (s, e) =>
        {
            if (e.PropertyName == nameof(IsUsersSelected))
            {
                if (IsUsersSelected)
                    await LoadUsersAsync();
                OnPropertyChanged(nameof(CurrentItems));
            }

            if (e.PropertyName == nameof(IsBooksSelected))
            {
                if (IsBooksSelected)
                    await LoadBooksAsync();
                OnPropertyChanged(nameof(CurrentItems));
            }
        };
    }

    [RelayCommand]
    private async Task LoadUsersAsync()
    {
        UserItems.Clear();

        var response = await _apiService.GetAllUsersAsync();

        if (response.Success && response.Data != null)
        {
            foreach (var user in response.Data)
            {
                UserItems.Add(new ListItemViewModel
                {
                    Id = user.Id,
                    Title = user.UserName,
                    SubTitle = user.Email,
                    IsUser = true
                });
            }
        }
    }

    [RelayCommand]
    private async Task LoadBooksAsync()
    {
        BookItems.Clear();

        var response = await _apiService.GetAllBooksAsync();

        if (response.Success && response.Data != null)
        {
            foreach (var book in response.Data)
            {
                BookItems.Add(new ListItemViewModel
                {
                    Id = book.Id.ToString(),
                    Title = book.Title,
                    SubTitle = book.Author,
                    IsUser = false,
                    CreatedDate = book.CreatedDate,
                    UpdatedDate = book.UpdatedDate
                });
            }
        }
    }

    [RelayCommand]
    private async Task DeleteItemAsync(ListItemViewModel item)
    {
        bool confirm = await Shell.Current.DisplayAlert("Silme Onayı", "Silmek istediğinize emin misiniz?", "Evet", "Hayır");

        if (!confirm)
            return;

        if (item.IsUser)
        {
            var response = await _apiService.DeleteUserAsync(item.Title);

            if (response.Success)
                UserItems.Remove(item);
        }
        else
        {
            var response = await _apiService.DeleteBookAsync(int.Parse(item.Id));

            if (response.Success)
                BookItems.Remove(item);
        }
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await _apiService.ClearTokenAsync();
        await Shell.Current.GoToAsync("//LoginPage");
    }
}