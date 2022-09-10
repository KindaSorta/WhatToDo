using System.Net.Sockets;
using WhatToDo.Data;

namespace WhatToDo.Service;

public class DataService : IDataService
{
    DataStorage dataStorage;

    Session Session;

    Dictionary<int, ToDoItem> Items { get; set; } = new Dictionary<int, ToDoItem>();

    Dictionary<DateOnly, WeatherData> WeatherForcast { get; set; } = new Dictionary<DateOnly, WeatherData>();

    public DataService()
    {
        dataStorage = new DataStorage();
        Task.Run(async () =>
        {
            Items = await GetItemsAsync();
            WeatherForcast = await GetWeatherAsync();
        });
    }

    #region Session
    public async Task SaveSession(Session session)
    {
        Session = await Task.FromResult(session);
    }

    public async Task<Session> GetSession()
    {
        return await Task.FromResult(Session ?? new Session());
    }
    #endregion Session

    #region TodoItems
    public async Task DeleteItemAsync(ToDoItem itemToDelete)
    {
        if (itemToDelete == null || itemToDelete.Id == 0)
            return;
        await DeleteItemAsync(new List<ToDoItem>() { itemToDelete });
    }

    public async Task DeleteItemAsync(List<ToDoItem> itemsToDelete)
    {
        foreach (var item in itemsToDelete)
        {
            if (item != null && item.Id != 0)
            {
                Items.Remove(item.Id);
            }
        }
        await dataStorage.WriteToFile(Items.Values.ToList(), "ToDoItems");
    }

    public async Task UpdateItemAsync(ToDoItem itemToUpdate)
    {
        if (itemToUpdate == null || String.IsNullOrEmpty(itemToUpdate.Name))
            return;            
        await UpdateItemAsync(new List<ToDoItem>() { itemToUpdate });
    }

    public async Task UpdateItemAsync(List<ToDoItem> itemsToUpdate)
    {
        foreach (var item in itemsToUpdate)
        {
            if (item != null && !String.IsNullOrEmpty(item.Name))
            {
                if (Items.ContainsKey(item.Id))
                {
                    Items[item.Id] = item;
                }
                else
                {
                    int newId = Items.Count > 0 ? Items.Keys.Max() + 1 : 1;
                    Items[newId] = item;
                    Items[newId].Id = newId;
                }
                
            }
        }
        await dataStorage.WriteToFile(Items.Values.ToList(), "ToDoItems");
    }

    public async Task<Dictionary<int, ToDoItem>> GetItemsAsync()
    {
        List<ToDoItem> data = await dataStorage.ReadFromFile<ToDoItem>("ToDoItems");
        foreach (var item in data)
        {
            if (item != null && !String.IsNullOrEmpty(item.Name) && item.Id != 0)
            {
                Items[item.Id] = item;
            }
        }
        return Items;
    }
    #endregion TodoItems

    #region Weather
    public async Task SaveWeatherData(Dictionary<DateOnly, WeatherData> forcast)
    {
        if (forcast != null && forcast.Count == 14)
        {
            WeatherForcast = forcast;
            await dataStorage.WriteToFile(WeatherForcast.Values.ToList(), "Weather");
        }
    }

    public async Task<Dictionary<DateOnly, WeatherData>> GetWeatherAsync()
    {
        List<WeatherData> data = await dataStorage.ReadFromFile<WeatherData>("Weather");
        if (data != null && data.Count == 14)
        {
            WeatherForcast.Clear();
            foreach (WeatherData halfDay in data)
            {
                WeatherForcast[DateOnly.FromDateTime(halfDay.startTime)] = halfDay;
            }
        }
        return WeatherForcast;
    }
    #endregion Weather
}
