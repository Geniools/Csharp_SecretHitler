using Microsoft.Maui.Controls.Platform;

namespace SecretHitler;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
    }
}
