using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KitapTakipMauii.Models.Dtos;
using KitapTakipMauii.Models.Responses;
using KitapTakipMauii.Services;

namespace KitapTakipMauii.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    [ObservableProperty]
    private string userName;

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string currentPassword;

    [ObservableProperty]
    private string newPassword;

    public ProfileViewModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    [RelayCommand]
    private async Task UpdateProfileAsync()
    {
        var updateProfileDto = new UpdateProfileDto
        {
            UserName = UserName,
            Email = Email
        };
        var response = await _apiService.UpdateProfileAsync(updateProfileDto);
        if (response.Success)
        {
            await Shell.Current.DisplayAlert("Başarılı", "Profil güncellendi.", "Tamam");
        }
        else
        {
            await Shell.Current.DisplayAlert("Hata", response.Message, "Tamam");
        }
    }

    [RelayCommand]
    private async Task ChangePasswordAsync()
    {
        var changePasswordDto = new ChangePasswordDto
        {
            UserName = UserName,
            CurrentPassword = CurrentPassword,
            NewPassword = NewPassword
        };
        var response = await _apiService.ChangePasswordAsync(changePasswordDto);
        if (response.Success)
        {
            await Shell.Current.DisplayAlert("Başarılı", "Şifre değiştirildi.", "Tamam");
        }
        else
        {
            await Shell.Current.DisplayAlert("Hata", response.Message, "Tamam");
        }
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await _apiService.ClearTokenAsync();
        await Shell.Current.GoToAsync("//LoginPage");
    }
}