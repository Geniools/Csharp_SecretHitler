﻿using SecretHitler.ViewModel;

namespace SecretHitler.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel vm)
	{
        InitializeComponent();
        BindingContext = vm;
	}
}

