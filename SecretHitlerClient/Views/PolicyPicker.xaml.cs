using SecretHitler.ViewModel;

namespace SecretHitler.Views;

public partial class PolicyPicker : ContentPage
{
	public PolicyPicker(PolicyPickerViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}