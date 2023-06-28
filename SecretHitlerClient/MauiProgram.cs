using CommunityToolkit.Maui;
using SecretHitler.Services;
using SecretHitler.ViewModel;
using SecretHitler.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

namespace SecretHitler;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureLifecycleEvents(AppLifecycle => {
                // TODO: Do smth when the application is being closed (not working at the moment)
                AppLifecycle.AddEvent("Closing", () => LogEvent("Closing"));
                AppLifecycle.AddEvent("Destroying", () => LogEvent("Destroying"));

                static bool LogEvent(string eventName, string type = null)
                {
                    System.Diagnostics.Debug.WriteLine($"Lifecycle event: {eventName}{(type == null ? string.Empty : $" ({type})")}");
                    return true;
                }
            })
            // Initialize the .NET MAUI Community Toolkit by adding the below line of code
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Plain-Germanica.ttf", "Germanica");
                fonts.AddFont("Shadowed-Germanica.ttf", "GermanicaBold");
            })
            .Services
            .AddViewModels()
            .AddViews()
            .AddSingleton<GameManager>();

        // This makes the app fullscreen on Windows
#if WINDOWS
        builder.ConfigureLifecycleEvents(events =>
        {
            // Make sure to add "using Microsoft.Maui.LifecycleEvents;" in the top of the file 
            events.AddWindows(windowsLifecycleBuilder =>
            {
                windowsLifecycleBuilder.OnWindowCreated(window =>
                {
                    window.ExtendsContentIntoTitleBar = false;
                    var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                    var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                    var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
                    switch (appWindow.Presenter)
                    {
                        case Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter:
                            overlappedPresenter.SetBorderAndTitleBar(false, false);
                            overlappedPresenter.Maximize();
                            break;
                    }
                });
            });
        });
#endif

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}