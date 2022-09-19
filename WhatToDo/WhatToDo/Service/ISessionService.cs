

namespace WhatToDo.Service;

public interface ISessionService
{
    GeolocCurrent CurrentGeolocInfo { get; set; }
    Dictionary<DateTime, WeatherData> WeatherForcast { get; set; }
    ToDoItem ItemToEdit { get; set; }
}
