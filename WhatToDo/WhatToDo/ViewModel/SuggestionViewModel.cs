

namespace WhatToDo.ViewModel;


public partial class SuggestionViewModel : BaseViewModel
{

    ISessionService session;

    [ObservableProperty]
    List<WeatherData> displayWeather;

    public SuggestionViewModel(ISessionService session)
    {
        this.session = session;
    }

    [RelayCommand]
    public async Task ShowWeatherAsync()
    {
        await Task.FromResult(DisplayWeather = session.WeatherForcast.Values.ToList());
    }

}
