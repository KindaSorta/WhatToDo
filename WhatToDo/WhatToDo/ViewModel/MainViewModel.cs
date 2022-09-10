

using System.Linq;
using WhatToDo.Service;
using WhatToDo.Data;
using WhatToDo.View;

namespace WhatToDo.ViewModel;

public partial class MainViewModel : BaseViewModel
{
    IDataService dataService;
    IDialogService dialogService;
    IGeolocation geolocation;

    public Session Session { get; set; }

    public ObservableCollection<ToDoItem> Items { get; } = new ();

    [ObservableProperty]
    ObservableCollection<ToDoItem> displayItems;

    [ObservableProperty]
    ToDoItem selectedItem;



    public MainViewModel(IDataService dataService, IDialogService dialogService, IGeolocation geolocation)
    {
        Title = "WhatToDo";
        SelectedItem = new ToDoItem();
        this.dataService = dataService;
        this.dialogService = dialogService;
        this.geolocation = geolocation;
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
            await GetDataAsync();
        }
        else
        {
            if (SelectedItem.Id == 0)
            {
                Items.Remove(SelectedItem);
            }
            SelectedItem = new ToDoItem();
        }

        //Get Session Data
        if (Session == null)
        {
            //TODO: Standardize Session and Data Service Call
            Session = await dataService.GetSession();
        }
        DisplayItems = null;
        DisplayItems = Items;
        IsRefreshing = false;
    }
    #endregion UI

    [RelayCommand]
    async Task GetDataAsync()
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

    public override async Task Cache()
    {
        await dataService.SaveSession(Session);
        return;
    }

    [RelayCommand]
    async Task GetGeoLoc()
    {
        if (IsBusy)
            return;
        try
        {
            IsBusy = true;
            // Get cached location, else get real location.
            Location location = await geolocation.GetLastKnownLocationAsync();
            if (location == null)
            {
                location = await geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(30)
                });
            }

        }
        catch { }
    }

    #region ToDo Item Details / Edits

    [RelayCommand]
    async Task Tap(ToDoItem item)
    {
        await Task.FromResult(SelectedItem = item);
        await GoToDetails(SelectedItem);
    }

    [RelayCommand]
    async Task AddItem()
    {
        Items.Add(SelectedItem);
        await GoToDetails(SelectedItem);
    }

    [RelayCommand]
    async Task DeleteAsync(ToDoItem item)
    {
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
