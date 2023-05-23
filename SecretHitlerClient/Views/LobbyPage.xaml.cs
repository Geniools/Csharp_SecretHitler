using SecretHitler.ViewModel;

namespace SecretHitler.Views;

public partial class LobbyPage : ContentPage
{
	public LobbyPage(LobbyPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}