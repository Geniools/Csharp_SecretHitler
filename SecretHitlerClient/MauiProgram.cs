using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SecretHitler.ViewModel;
using SecretHitler.Views;

namespace SecretHitler;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            // Initialize the .NET MAUI Community Toolkit by adding the below line of code
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MainViewModel>();
        
        #if DEBUG
        	builder.Logging.AddDebug();
        #endif
        
        return builder.Build();
    }
}
