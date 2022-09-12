﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToDo.Service
{
    public interface IDataService
    {
/*        Task SaveSession(ISessionService session);
        Task<ISessionService> GetSession();*/
        Task DeleteItemAsync(ToDoItem itemToDelete); 
        Task DeleteItemAsync(List<ToDoItem> itemsToDelete);
        Task<Dictionary<int, ToDoItem>> GetItemsAsync(); 
        Task UpdateItemAsync(ToDoItem itemToUpdate);
        Task UpdateItemAsync(List<ToDoItem> itemsToUpdate);
        Task SaveWeatherData();
        Task<Dictionary<DateTime, WeatherData>> GetWeatherAsync();
    }
}
