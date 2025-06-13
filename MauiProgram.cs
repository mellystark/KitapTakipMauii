using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using KitapTakipMauii.Services;
using KitapTakipMauii.ViewModels;

namespace KitapTakipMauii;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Servis kayıtları
        builder.Services.AddHttpClient();
        builder.Services.AddSingleton<ApiService>();

        // ViewModel kayıtları
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<BooksViewModel>();
        builder.Services.AddTransient<BookAddViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();

        // Page kayıtları
        builder.Services.AddTransient<Pages.LoginPage>();
        builder.Services.AddTransient<Pages.RegisterPage>();
        builder.Services.AddTransient<Pages.BooksPage>();
        builder.Services.AddTransient<Pages.BookAddPage>();
        builder.Services.AddTransient<Pages.ProfilePage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}