using SecretHitler.ViewModel;

namespace SecretHitler.Views;

public partial class StartPage : ContentPage
{
    public StartPage(StartPageViewModel vm)
	{
        InitializeComponent();
        BindingContext = vm;
	}
}

