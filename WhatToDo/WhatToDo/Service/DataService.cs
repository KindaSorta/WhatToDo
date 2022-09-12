using System.Net.Sockets;
using WhatToDo.Data;

namespace WhatToDo.Service;

public class DataService : IDataService
{
    ISessionService session;
    FileStorage dataStorage;

    Dictionary<int, ToDoItem> Items { get; set; } = new Dictionary<int, ToDoItem>();    

    public DataService(ISessionService session)
    {
        this.session = session;

        dataStorage = new FileStorage();
        Task.Run(async () =>
        {
            Items = await GetItemsAsync();
            session.WeatherForcast = await GetWeatherAsync();
        });
    }

    #region Session
    /*    public async Task SaveSession(SessionService session)
        {
            Session = await Task.FromResult(session);
        }

        public async Task<SessionService> GetSession()
        {
            return await Task.FromResult(Session ?? new SessionService());
        }*/
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
    public async Task SaveWeatherData()
    {
        if (session.WeatherForcast is not null && session.WeatherForcast.Count > 0)
        {
            await dataStorage.WriteToFile(session.WeatherForcast.Values.ToList(), "Weather");
        }
    }

    public async Task<Dictionary<DateTime, WeatherData>> GetWeatherAsync()
    {
        List<WeatherData> data = await dataStorage.ReadFromFile<WeatherData>("Weather");
        if (data != null && data.Count == 14)
        {
            session.WeatherForcast.Clear();
            foreach (WeatherData halfDay in data)
            {
                session.WeatherForcast[halfDay.startTime] = halfDay;
            }
        }
        return session.WeatherForcast;
    }
    #endregion Weather
}
