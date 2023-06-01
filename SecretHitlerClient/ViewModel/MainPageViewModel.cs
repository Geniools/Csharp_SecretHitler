using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SecretHitler.Model;

namespace SecretHitler.ViewModel
{
    public partial class MainPageViewModel : ViewModel
    {
        [ObservableProperty]
        private string _username;

        public MainPageViewModel(GameManager gameManager) : base(gameManager) { }


    }
}
