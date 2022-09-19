

using WhatToDo.Data;
using WhatToDo.Model;

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
    string header;

    [ObservableProperty]
    string filter;

    [ObservableProperty]
    IList<IndexedToDoItem> displayItems;

    [ObservableProperty]
    bool isPopUp;

    [ObservableProperty]
    float blur;

    public MainViewModel(
        IDataService dataService, IDialogService dialogService, 
        IGeolocation geolocation, ISessionService session, IConnectivity connectivity, WeatherService weatherService)
    {
        this.dataService = dataService;
        this.dialogService = dialogService;
        this.geolocation = geolocation;
        this.connectivity = connectivity;
        this.weatherService = weatherService;
        this.session = session;
        Task.FromResult(InitialLoad());
    }

    #region Navigation

    async Task GoToDetails()
    {
        await Shell.Current.GoToAsync(nameof(ToDoItemDetailsPage), true);
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
            if (session.ItemToEdit.Id == 0)
            {
                Items.Remove(session.ItemToEdit);
            }
            session.ItemToEdit = new ToDoItem();
        }

        if (DisplayItems is null) await IncompleteTasksAsync();
        
        IList<ToDoItem> items = await FilterQueryItems(DisplayItems.Select(x => x.BackToObject()));
        DisplayItems = null;
        DisplayItems = FormatDisplayObject(Items.ToList());

        IsRefreshing = false;
    }

    #region Filtering

    async Task<IList<ToDoItem>> FilterQueryItems(IEnumerable<ToDoItem> items)
    {
        // TODO: Make it handle nested search queries, and Tag filtering

        switch (Filter)
        {
            // Recent
            case var s when s == Options[1]:
                return await Task.FromResult(items.OrderBy(x => x.LastModifiedDate).ToList());

            // Upcoming
            case var s when s == Options[2]:
                return await Task.FromResult(items.OrderBy(x => x.DueDate).ToList());

            // Priority
            case var s when s == Options[3]:
                return await Task.FromResult(items.OrderByDescending(x => x.Priority).ToList());

            // Urgency
            case var s when s == Options[4]:
                return await Task.FromResult(items.OrderBy(x => x.DueDate - DateTime.Now).OrderByDescending(x => x.Priority).ToList());

            // Suggested
            case var s when s == Options[5]:
                return await Task.FromResult(items.OrderBy(x => x.SuggestedDate.Count != 0 ? x.SuggestedDate[0] - DateTime.Now : x.DueDate - DateTime.Now).OrderByDescending(x => x.Priority).ToList());

            default:
                break;
        }
        return items.ToList();
    }

    static IList<IndexedToDoItem> FormatDisplayObject(IList<ToDoItem> items)
    {
        IList<IndexedToDoItem> numberedList = new ObservableCollection<IndexedToDoItem>();
        if (items.Count > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                numberedList.Add(new IndexedToDoItem(i + 1, items[i]));
            }
        }
        return numberedList;
    }

    [RelayCommand]
    async Task CompletedTasksAsync()
    {
        Header = "Completed";
        IList<ToDoItem> items = await Task.FromResult(Items.Where(x => x.IsComplete).ToList());
        DisplayItems = FormatDisplayObject(items);
    }

    [RelayCommand]
    async Task IncompleteTasksAsync()
    {
        Header = "Incomplete";
        IList<ToDoItem> items = await Task.FromResult(Items.Where(x => x.NotComplete).ToList());
        DisplayItems = FormatDisplayObject(items);
    }

    #endregion Filtering

    #endregion UI

    #region Get Data

    public async Task InitialLoad()
    {

        await dataService.GetSessionAsync(session);

        if (session.CurrentGeolocInfo.Location == null || DateTime.Now >= session.CurrentGeolocInfo.NextUpdateTime)
        {
            await GetGeoLocAsync();
            await dataService.SaveLastPositionData(session.CurrentGeolocInfo);
        }

        if (session.CurrentGeolocInfo.Location != null && session.WeatherForcast == null || session.WeatherForcast.Count <= 0)
        {
            await GetWeatherAsync();
            await dataService.SaveWeatherData(session.WeatherForcast.Values.ToList());
        }

        if (Items == null || Items.Count == 0)
        {
            await GetItemsAsync();
        }

        Header = "Incomplete";
        Filter = Options[2];
        Blur = 1;
        IsRefreshing = true;
    }

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

            if(location is not null)
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

    [RelayCommand]
    async Task GetWeatherAsync()
    {
        if (IsBusy)
            return;
        try
        {
            IsBusy = true;
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
                        await Task.FromResult(session.WeatherForcast[halfDay.startTime] = halfDay);
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
    void FilterSelector(string selection)
    {
        Filter = selection;
        IsPopUp = !IsPopUp;
        Blur = IsPopUp ? 0.4f : 1f;
        IsRefreshing = !IsPopUp;
    }

    [RelayCommand]
    async Task TapAsync(IndexedToDoItem item)
    {
        if (IsBusy) return;
        session.ItemToEdit = item.Value;
        await GoToDetails();
    }

    [RelayCommand]
    async Task AddAsync()
    {
        if (IsBusy) return;
        Items.Add(session.ItemToEdit);
        await GoToDetails();
    }

    [RelayCommand]
    async Task ToggoleCompleteAsync(IndexedToDoItem item)
    {
        if (IsBusy) return;
        item.Value.IsComplete = !item.Value.IsComplete;
        item.Value.LastModifiedDate = DateTime.Now;
        await dataService.UpdateItemAsync(item.Value);
        DisplayItems.Remove(item);
        IsRefreshing = true;
    }

    [RelayCommand]
    async Task DeleteAsync(IndexedToDoItem item)
    {
        if (IsBusy) return;

        bool answer = await dialogService.DisplayAlert("Alert!!", "You are about to Delete a Task! Please confirm...", "OK", "Cancel");
        if (!answer) return;

        await dataService.DeleteItemAsync(item.Value);
        Items.Remove(item.Value);
        DisplayItems.Remove(item);
    }

    public override void Dispose() 
    {
        base.Dispose();
        
    }

    #endregion ToDo Item Details / Edits
}
