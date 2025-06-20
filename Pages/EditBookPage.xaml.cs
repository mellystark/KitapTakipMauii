using KitapTakipMauii.ViewModels;

namespace KitapTakipMauii.Pages;

public partial class EditBookPage : ContentPage
{
    public EditBookPage(EditBookViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is EditBookViewModel viewModel)
        {
            await viewModel.LoadBookAsync();
        }
    }

    protected override bool OnBackButtonPressed()
    {
        if (BindingContext is EditBookViewModel vm && vm.IsBusy)
        {
            System.Diagnostics.Debug.WriteLine("IsBusy engelliyor");
            return false; // Geri tuþunu engelle
        }
        Shell.Current.GoToAsync("..");
        return true;
    }
}