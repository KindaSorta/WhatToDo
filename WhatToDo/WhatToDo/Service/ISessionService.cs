

namespace WhatToDo.Service;

public interface ISessionService
{
    GeolocCurrent CurrentGeolocInfo { get; set; }
    List<WeatherData> WeatherForcast { get; set; }
    ToDoItem ItemToEdit { get; set; }
    DateTime LastUpdated { get; set; }  
}
