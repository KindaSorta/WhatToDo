

using System.Linq;
using WhatToDo.Service;
using WhatToDo.Data;
using WhatToDo.View;

namespace WhatToDo.ViewModel;

public partial class MainViewModel : BaseViewModel
{
    [ObservableProperty]
    ObservableCollection<ToDoItem> items;

    DataStorage dataStorage;

    [ObservableProperty]
    bool isRefreshing;

    public MainViewModel(DataStorage dataStorage)
    {
        Title = "WhatToDo";
        this.dataStorage = dataStorage;
    }

    async Task GoToDetails(ToDoItem item)
    {
        if (item == null) return;
        await Shell.Current.GoToAsync(nameof(ToDoItemDetailsPage), true, new Dictionary<string, object> { { "item", item } });
    }

    [RelayCommand]
    async Task Tap(ToDoItem item)
    {
        await GoToDetails(item);
        isRefreshing = true;
    }

    [RelayCommand]
    async Task AddItem()
    {
        ToDoItem item = new ToDoItem();
        await GoToDetails(item);
    }

    [RelayCommand]
    async Task GetDataAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;

            Dictionary<string, ToDoItem> data = await dataStorage.GetItemsAsync();

            if (data != null)
            {
                Items = new ObservableCollection<ToDoItem>(data.Values);
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
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    async Task DeleteAsync(ToDoItem item)
    {
        if (Items.Contains(item))
        {
            await dataStorage.DeleteItemAsync(item);
            Items.Remove(item);
        }
        isRefreshing = true;
    }
}
