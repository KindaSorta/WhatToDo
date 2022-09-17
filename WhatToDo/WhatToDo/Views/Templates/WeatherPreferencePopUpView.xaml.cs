

//using AndroidX.Lifecycle;
using WhatToDo.Data;

namespace WhatToDo.Views.Templates;

public partial class WeatherPreferencePopUpView : ContentView
{
	public WeatherPreferencePopUpViewModel _viewModel;

	public WeatherPreferencePopUpView(WeatherPreferencePopUpViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }
}