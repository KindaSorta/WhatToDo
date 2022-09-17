using WhatToDo.Model;
using WhatToDo.Service;

namespace WhatToDo.Views.Pages;

public partial class MainPage : ContentPage
{
    private MainViewModel _viewModel;

    public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = _viewModel = viewModel;
        
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.IsRefreshing = true;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.IsCaching = true;
    }
}

