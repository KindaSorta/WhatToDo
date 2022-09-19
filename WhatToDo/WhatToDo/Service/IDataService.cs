using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToDo.Service
{
    public interface IDataService
    {
        Task<ISessionService> GetSessionAsync(ISessionService session);
        Task SaveSession(ISessionService session);

        Task<Dictionary<int, ToDoItem>> GetItemsAsync(); 
        Task UpdateItemAsync(ToDoItem itemToUpdate);
        Task UpdateItemAsync(List<ToDoItem> itemsToUpdate);
        Task DeleteItemAsync(ToDoItem itemToDelete);
        Task DeleteItemAsync(List<ToDoItem> itemsToDelete);

        Task<GeolocCurrent> GetLastPositionAsync();
        Task SaveLastPositionData(GeolocCurrent position);

        Task<Dictionary<DateTime, WeatherData>> GetWeatherAsync();
        Task SaveWeatherData(List<WeatherData> forecast);
    }
}
