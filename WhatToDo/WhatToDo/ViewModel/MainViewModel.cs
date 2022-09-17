

using WhatToDo.Data;

namespace WhatToDo.ViewModel;

public partial class MainViewModel : BaseViewModel
{
    IDataService dataService;
    IDialogService dialogService;
    IGeolocation geolocation;
    IConnectivity connectivity;
    ISessionService session;
    WeatherService weatherService;

    public List<string> Options { get; } = FormOptions.FilterOptions;

    public ObservableCollection<ToDoItem> Items { get; } = new ();

    [ObservableProperty]
    string filter;

    [ObservableProperty]
    IList<ToDoItem> displayItems;

    [ObservableProperty]
    ToDoItem selectedItem;

    [ObservableProperty]
    bool isPopUp;

    [ObservableProperty]
    float blur;

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
        await Shell.Current.GoToAsync(nameof(ToDoItemDetailsPage), true, new Dictionary<string, object> { { "CurrentItem", item } });
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
        DisplayItems = await FilterQueryItems();
        IsRefreshing = false;
    }


    async Task<IList<ToDoItem>> FilterQueryItems()
    {
        // TODO: Make it handle nested search queries, and Tag filtering
        switch (Filter)
        {
            // Complete
            case var s when s == Options[1]:
                return await Task.FromResult(Items.Where(x => x.IsComplete).ToList());

            // Incomplete
            case var s when s == Options[2]:
                return await Task.FromResult(Items.Where(x => x.NotComplete).ToList());

            // Recent
            case var s when s == Options[3]:
                break;

            // Upcoming
            case var s when s == Options[4]:
                return await Task.FromResult(Items.OrderBy(x => x.DueDate).ToList());

            // Priority
            case var s when s == Options[5]:
                return await Task.FromResult(Items.OrderByDescending(x => x.Priority).ToList());
            default:
                return Items;
        }
        
        if (Filter == "Completed")
        {
            return await Task.FromResult(Items.Where(x => x.IsComplete).ToList());
        }
        else
        {
            return await Task.FromResult(Items);
        }
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

    /*    [RelayCommand]
        async Task TapAsync(ToDoItem item)
        {
            *//*        if (IsBusy) return;
                    await Task.FromResult(SelectedItem = item);
                    await GoToDetails(SelectedItem);*//*
            return;
        }*/
    [RelayCommand]
    async Task FilterAsync()
    {
        await Task.FromResult(IsPopUp = !IsPopUp);
        await Task.FromResult(Blur = IsPopUp ? 0.4f : 1f);
        IsRefreshing = !IsPopUp;
    }

    [RelayCommand]
    async Task EditAsync(ToDoItem item)
    {
        if (IsBusy) return;
        await Task.FromResult(SelectedItem = item);
        await GoToDetails(SelectedItem);
    }

    [RelayCommand]
    async Task AddAsync()
    {
        if (IsBusy) return;
        Items.Add(SelectedItem);
        await GoToDetails(SelectedItem);
    }

    [RelayCommand]
    async Task ToggoleCompleteAsync(ToDoItem item)
    {
        if (IsBusy) return;
        item.IsComplete = !item.IsComplete;
        await dataService.UpdateItemAsync(item);
        //await Task.Delay(200);
        IsRefreshing = true;
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
