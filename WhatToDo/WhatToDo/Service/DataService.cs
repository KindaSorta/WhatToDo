using System.Net.Sockets;
using WhatToDo.Data;

namespace WhatToDo.Service;

public class DataService : IDataService
{
    FileStorage dataStorage;

    Dictionary<int, ToDoItem> Items { get; set; } = new Dictionary<int, ToDoItem>();    

    public DataService()
    {
        dataStorage = new FileStorage();
    }

    #region Session

    #region Get Session Data

    public async Task<ISessionService> GetSessionAsync(ISessionService session)
    {
        session.CurrentGeolocInfo = await GetLastPositionAsync();
        session.WeatherForcast = await GetWeatherAsync();
        return session;
    }

    #endregion Get Session Data

    #region Update Session Data

    public async Task SaveSession(ISessionService session)
    {
        await SaveLastPositionData(session.CurrentGeolocInfo);
        await SaveWeatherData(session.WeatherForcast.Values.ToList());
    }

    #endregion Update Session Data

    #endregion Session

    #region TodoItems

    #region Get ToDoItems

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

    #endregion Get ToDoItems

    #region Update Data

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

    #endregion Update ToDoItems

    #region Delete ToDoItems

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

    #endregion Delete ToDoItems

    #endregion TodoItems

    #region Geolocation

    #region Get Last Location

    public async Task<GeolocCurrent> GetLastPositionAsync()
    {
        List<GeolocCurrent> data = await dataStorage.ReadFromFile<GeolocCurrent>("Position");
        if (data != null && data.Count == 1)
        {
            return data[0];
        }
        return new GeolocCurrent();
    }

    #endregion Get Last Location

    #region Update Last Location

    public async Task SaveLastPositionData(GeolocCurrent position)
    {
        if (position is not null)
        {
            await dataStorage.WriteToFile(new List<GeolocCurrent>() { position }, "Position");
        }
    }

    #endregion Update Last Location

    #endregion Geolocation

    #region Weather

    #region Get Weather Data

    public async Task<Dictionary<DateTime, WeatherData>> GetWeatherAsync()
    {
        Dictionary<DateTime, WeatherData> output = new Dictionary<DateTime, WeatherData>();
        List<WeatherData> data = await dataStorage.ReadFromFile<WeatherData>("Weather");
        if (data != null && data.Count > 0)
        {            
            foreach (WeatherData halfDay in data)
            {
                output[halfDay.startTime] = halfDay;
            }
        }
        return output;
    }

    #endregion Get Weather Data

    #region Update Weather Data

    public async Task SaveWeatherData(List<WeatherData> forecast)
    {
        if (forecast is not null && forecast.Count > 0)
        {
            await dataStorage.WriteToFile(forecast, "Weather");
        }
    }

    #endregion Update Weather Data

    #endregion Weather
}
