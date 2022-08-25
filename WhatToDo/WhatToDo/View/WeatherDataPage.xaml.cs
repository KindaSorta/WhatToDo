using WhatToDo.Service;

namespace WhatToDo.View;

public partial class WeatherDataPage : ContentPage
{
    WeatherService weatherService = new WeatherService();
    ObservableCollection<WeatherData> data = new ObservableCollection<WeatherData>();

    public WeatherDataPage()
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