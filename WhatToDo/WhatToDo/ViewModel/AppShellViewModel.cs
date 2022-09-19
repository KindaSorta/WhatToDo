
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToDo.ViewModel;

public partial class AppShellViewModel : BaseViewModel
{
    IDataService dataService;
    IDialogService dialogService;
    IGeolocation geolocation;
    IConnectivity connectivity;
    ISessionService session;
    WeatherService weatherService;


    public AppShellViewModel(
        IDataService dataService, 
        IDialogService dialogService,
        IGeolocation geolocation, 
        ISessionService session, 
        IConnectivity connectivity, 
        WeatherService weatherService)
    {
        this.dataService = dataService;
        this.dialogService = dialogService;
        this.geolocation = geolocation;
        this.connectivity = connectivity;
        this.weatherService = weatherService;
        this.session = session;
        //Task.FromResult(InitialLoad());
    }

    public async Task InitialLoad()
    {
        if (session.CurrentGeolocInfo.Location == null || DateTime.Now >= session.CurrentGeolocInfo.NextUpdateTime)
        {
            //await GetGeoLocAsync();
        }

        if (session.CurrentGeolocInfo.Location != null && (session.WeatherForcast == null || session.WeatherForcast.Count == 0))
        {
            //await GetWeatherAsync();
            //await dataService.SaveWeatherData();
        }
    }


    async Task GetGeoLocAsync()
    {
        if (IsBusy)
            return;
        try
        {
            IsBusy = true;
            // Get cached location, else get real location.
            Location location = await geolocation.GetLastKnownLocationAsync();
            if (location is null || DateTime.Now > session.CurrentGeolocInfo.NextUpdateTime)
            {
                location = await geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(30)
                });

                if (location is null)
                {
                    throw new ArgumentNullException(nameof(location));
                }
            }

            session.CurrentGeolocInfo.Location = location;

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get Updated Location: {ex.Message}");
            await dialogService.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally { IsBusy = false; }
    }


    async Task GetWeatherAsync()
    {
        if (IsBusy)
            return;
        try
        {
            if (session.WeatherForcast is null || session.WeatherForcast.Count < 14 || session.WeatherForcast.Keys.Any(x => x < DateTime.Now.AddDays(-1)))
            {
                if (connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await dialogService.DisplayAlert("No connectivity!", "Please check internet and try again.", "OK");
                    return;
                }

                List<WeatherData> data = await weatherService.GetWeather(session.CurrentGeolocInfo.Location);

                if (data is null || data.Count == 0)
                {
                    throw new ArgumentNullException(nameof(data));
                }
                else
                {
                    session.WeatherForcast.Clear();
                    foreach (WeatherData halfDay in data)
                    {
                        session.WeatherForcast[halfDay.startTime] = halfDay;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get Weather Data: {ex.Message}");
            await dialogService.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

}
