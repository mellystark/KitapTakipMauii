using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KitapTakipMauii.Models.Dtos;
using KitapTakipMauii.Models.Responses;
using KitapTakipMauii.Services;
using System.Collections.ObjectModel;

namespace KitapTakipMauii.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    [ObservableProperty]
    private string userNameOrEmail;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private ObservableCollection<string> loginTypes = new() { "Kullanıcı", "Admin" };

    [ObservableProperty]
    private string selectedLoginType = "Kullanıcı";

    public LoginViewModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        try
        {
            var loginDto = new LoginDto
            {
                UserNameOrEmail = UserNameOrEmail,
                Password = Password
            };

            ApiResponse<string> response;

            if (SelectedLoginType == "Admin")
            {
                response = await _apiService.LoginAdminAsync(loginDto);
            }
            else
            {
                response = await _apiService.LoginAsync(loginDto);
            }

            Console.WriteLine($"Giriş Durumu: Success={response.Success}, Message={response.Message}");

            if (response.Success)
            {
                // DÜZELTİLEN ROUTE
                await Shell.Current.GoToAsync("//MainTabs/BooksPage");
            }
            else
            {
                await Shell.Current.DisplayAlert("Hata", response.Message, "Tamam");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Beklenmeyen Hata", ex.Message, "Tamam");
            Console.WriteLine("Login Hatası: " + ex);
        }
    }

    [RelayCommand]
    private async Task NavigateToRegisterAsync()
    {
        await Shell.Current.GoToAsync("RegisterPage");
    }
}
