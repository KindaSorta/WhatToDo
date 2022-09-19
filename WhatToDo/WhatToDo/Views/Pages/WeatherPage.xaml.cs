namespace WhatToDo.Views.Pages;

public partial class WeatherPage : ContentPage
{
	public WeatherPage(WeatherViewModel viewmodel)
	{
		InitializeComponent();
		BindingContext = viewmodel;
	}
}