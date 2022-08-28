

using System.Linq;
using WhatToDo.Service;
using WhatToDo.Data;
using WhatToDo.View;

namespace WhatToDo.ViewModel;

public partial class MainViewModel : BaseViewModel
{
    IDataService dataService;


    public ObservableCollection<ToDoItem> Items { get; } = new ();

    [ObservableProperty]
    ObservableCollection<ToDoItem> displayItems;

    [ObservableProperty]
    ToDoItem selectedItem; 

    public MainViewModel(IDataService dataService)
    {
        Title = "WhatToDo";
        this.dataService = dataService;
        SelectedItem = new ToDoItem();
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
        if (Items == null || Items.Count == 0)
        {
            await GetDataAsync();
        }
        else
        {
            if (String.IsNullOrWhiteSpace(SelectedItem.Name))
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

    [RelayCommand]
    async Task GetDataAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;

            Dictionary<string, ToDoItem> data = await dataService.GetItemsAsync();

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
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false; 
        }
    }

    #region ToDo Item Details / Edits

    [RelayCommand]
    async Task Tap(ToDoItem item)
    {
        SelectedItem = item;
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

    #endregion ToDo Item Details / Edits
}
