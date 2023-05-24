using SecretHitler.ViewModel;

namespace SecretHitler.Views;

public partial class JoinGamePage : ContentPage
{
	public JoinGamePage(JoinGamePageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}