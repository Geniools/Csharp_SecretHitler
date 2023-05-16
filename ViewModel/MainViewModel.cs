using CommunityToolkit.Mvvm.ComponentModel;

namespace SecretHitler.ViewModel;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    string _text;
}
