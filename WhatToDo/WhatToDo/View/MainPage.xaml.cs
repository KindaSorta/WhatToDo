using WhatToDo.Service;

namespace WhatToDo.View;

public partial class MainPage : ContentPage
{
	WeatherService weatherService = new WeatherService();
    ObservableCollection<WeatherData> data = new ObservableCollection<WeatherData>();

    public MainPage()
	{
		InitializeComponent();
		Start();
	}

	public async Task Start()
	{
		data = new ObservableCollection<WeatherData>(await weatherService.GetWeather());
		dataDisplay.ItemsSource = data;
    }

}

