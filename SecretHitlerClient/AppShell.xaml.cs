using SecretHitler.Views;


namespace SecretHitler;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(StartPage), typeof(StartPage));
        Routing.RegisterRoute(nameof(JoinGamePage), typeof(JoinGamePage));
        Routing.RegisterRoute(nameof(LobbyPage), typeof(LobbyPage));
        Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
    }
}
