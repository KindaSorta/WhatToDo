using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToDo.Service
{
    public interface IDataService
    {
        /*
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
        */


        Task<List<ToDoItem>> GetAllItems();
        //Task SelectItem(ToDoItem itemToUpdate);
        //Task SelectManyItems(ToDoItem itemToUpdate);
        Task SaveItem(ToDoItem item);
        Task SaveManyItem(List<ToDoItem> items);
        Task DeleteItem(ToDoItem item);
        //Task DeleteManyItem(List<ToDoItem> item);

        Task<List<GeolocCurrent>> GetGeolocation();
        Task SaveGeolocation(GeolocCurrent geoloc);
        Task DeleteAllGeolocation();

        Task<List<WeatherData>> GetAllWeatherData();
        Task SaveWeatherData(WeatherData data);
        Task SaveManyWeatherData(List<WeatherData> items);
        Task DeleteAllWeatherData(WeatherData data);

        Task SaveSession(SessionService session);
    }
}
