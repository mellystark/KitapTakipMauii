using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KitapTakipMauii.Models.Dtos;
using KitapTakipMauii.Models.Responses;
using KitapTakipMauii.Services;
using System.Collections.ObjectModel;

namespace KitapTakipMauii.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    [ObservableProperty]
    private string userName;

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private ObservableCollection<string> roles = new() { "User", "Admin" };

    [ObservableProperty]
    private string selectedRole = "User";

    public RegisterViewModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    [RelayCommand]
    private async Task RegisterAsync()
    {
        var registerDto = new RegisterDto
        {
            UserName = UserName,
            Email = Email,
            Password = Password,
            Role = SelectedRole
        };

        var response = await _apiService.RegisterAsync(registerDto);
        if (response.Success)
        {
            await Shell.Current.DisplayAlert("Başarılı", "Kayıt başarılı!", "Tamam");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await Shell.Current.DisplayAlert("Hata", response.Message, "Tamam");
        }
    }

    [RelayCommand]
    private async Task NavigateBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}