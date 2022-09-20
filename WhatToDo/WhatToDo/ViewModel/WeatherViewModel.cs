using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToDo.ViewModel
{
    public partial class WeatherViewModel : BaseViewModel
    {

        ISessionService session;

        [ObservableProperty]
        List<WeatherData> displayWeather;

        public WeatherViewModel(ISessionService session)
        {
            this.session = session;
        }

        [RelayCommand]
        public async Task ShowWeatherAsync()
        {
            await Task.FromResult(DisplayWeather = session.WeatherForcast);
        }
    }
}
