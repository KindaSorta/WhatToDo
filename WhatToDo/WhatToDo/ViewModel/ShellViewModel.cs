

//using IntelliJ.Lang.Annotations;

namespace WhatToDo.ViewModel;

public partial class ShellViewModel : BaseViewModel 
{

    IDataService dataService;
    IDialogService dialogService;
    IGeolocation geolocation;
    IConnectivity connectivity;
    ISessionService session;
    WeatherService weatherService;

    public ShellViewModel(IDataService dataService, IDialogService dialogService,
        IGeolocation geolocation, ISessionService session, IConnectivity connectivity, WeatherService weatherService)
    {
        this.dataService = dataService;
        this.dialogService = dialogService;
        this.geolocation = geolocation;
        this.connectivity = connectivity;
        this.weatherService = weatherService;
        this.session = session;
    }

/*    async Task GoToMainPage()
    {
        await Shell.Current.GoToAsync(nameof(MainPage), true);
    }*/


    public async Task InitialLoad()
    {
        List<GeolocCurrent> geoloc = await dataService.GetGeolocation();
        List<WeatherData> forecast = await dataService.GetAllWeatherData();

        if (geoloc != null && geoloc.Count > 0) session.CurrentGeolocInfo = geoloc[0];
        if (forecast != null && forecast.Count > 0) session.WeatherForcast = forecast;

        if (session.CurrentGeolocInfo.Location == null || DateTime.Now >= session.CurrentGeolocInfo.NextUpdateTime)
        {
            await GetGeoLocAsync();
            await dataService.SaveGeolocation(session.CurrentGeolocInfo);
        }

        if (session.CurrentGeolocInfo.Location != null && session.WeatherForcast == null || session.WeatherForcast.Count <= 0)
        {
            await GetWeatherAsync();
            await dataService.SaveManyWeatherData(session.WeatherForcast);
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

            if (location is not null)
            {
                session.CurrentGeolocInfo.Location = location;
            }


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
            IsBusy = true;
            if (session.WeatherForcast is null || session.WeatherForcast.Count < 14 || session.WeatherForcast.Any(x => x.startTime < DateTime.Now.AddDays(-1)))
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
                        session.WeatherForcast.Add(halfDay);
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
