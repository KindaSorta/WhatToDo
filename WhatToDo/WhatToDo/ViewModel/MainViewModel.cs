

namespace WhatToDo.ViewModel;

public partial class MainViewModel : BaseViewModel
{
    IDataService dataService;
    IDialogService dialogService;
    IGeolocation geolocation;
    IConnectivity connectivity;
    ISessionService session;
    WeatherService weatherService;

    public ObservableCollection<ToDoItem> Items { get; } = new ();

    [ObservableProperty]
    ObservableCollection<ToDoItem> displayItems;

    [ObservableProperty]
    ToDoItem selectedItem;

    public MainViewModel(
        IDataService dataService, IDialogService dialogService, 
        IGeolocation geolocation, ISessionService session, IConnectivity connectivity, WeatherService weatherService)
    {
        Title = "WhatToDo";
        SelectedItem = new ToDoItem();
        this.dataService = dataService;
        this.dialogService = dialogService;
        this.geolocation = geolocation;
        this.connectivity = connectivity;
        this.weatherService = weatherService;
        this.session = session;
        Task.FromResult(InitialLoad());
    }

    public async Task InitialLoad()
    {
        if (session.CurrentGeolocInfo.Location == null || DateTime.Now >= session.CurrentGeolocInfo.NextUpdateTime)
        {
            await GetGeoLocAsync();
        }

        if (session.CurrentGeolocInfo.Location != null && (session.WeatherForcast == null || session.WeatherForcast.Count == 0))
        {
            await GetWeatherAsync();
            await dataService.SaveWeatherData();
        }

        if (Items == null || Items.Count == 0)
        {
            await GetItemsAsync();
        }
    }

    #region Navigation
    async Task GoToDetails(ToDoItem item)
    {
        if (item == null) return;
        await Shell.Current.GoToAsync(nameof(ToDoItemDetailsPage), true, new Dictionary<string, object> { { "item", item } });
    }
    #endregion Navigation

    #region UI
    [RelayCommand]
    async Task RefreshAsync()
    {

        //Get Todo Item Data
        if (Items == null || Items.Count == 0)
        {
            await GetItemsAsync();
        }
        else
        {
            if (SelectedItem.Id == 0)
            {
                Items.Remove(SelectedItem);
            }
            SelectedItem = new ToDoItem();
        }

        DisplayItems = null;
        DisplayItems = Items;
        IsRefreshing = false;
    }
    #endregion UI

    #region Get Data
    [RelayCommand]
    async Task GetItemsAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;

            Dictionary<int, ToDoItem> data = await dataService.GetItemsAsync();

            if (data != null)
            {
                Items.Clear();
                foreach (var item in data.Values)
                {
                    Items.Add(item);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get todo items: {ex.Message}");
            await dialogService.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false; 
        }
    }

/*    public override async Task Cache()
    {
        await dataService.SaveSession(session);
        return;
    }*/

    [RelayCommand]
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

    [RelayCommand]
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
    #endregion Get Data

    #region ToDo Item Details / Edits

    [RelayCommand]
    async Task Tap(ToDoItem item)
    {
        if (IsBusy) return;
        await Task.FromResult(SelectedItem = item);
        await GoToDetails(SelectedItem);
    }

    [RelayCommand]
    async Task AddItem()
    {
        if (IsBusy) return;
        Items.Add(SelectedItem);
        await GoToDetails(SelectedItem);
    }

    [RelayCommand]
    async Task CompleteAsync(ToDoItem item)
    {
        if (IsBusy) return;
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].Id == item.Id)
            {
                Items[i].IsComplete = !item.IsComplete;
                break;
            }
        }
        await dataService.UpdateItemAsync(item);
        await Task.Delay(200);
        DisplayItems = null;
        DisplayItems = Items;        
    }

    [RelayCommand]
    async Task DeleteAsync(ToDoItem item)
    {
        if (IsBusy) return;
        if (Items.Contains(item))
        {
            await dataService.DeleteItemAsync(item);
            Items.Remove(item);
        }
    }

    public override void Dispose() 
    { 

        base.Dispose();
    }

    #endregion ToDo Item Details / Edits
}
