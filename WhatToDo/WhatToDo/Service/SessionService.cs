

namespace WhatToDo.Service;

public class SessionService : ISessionService
{
    //public int Id { get; set; }

    public GeolocCurrent CurrentGeolocInfo { get; set; } = new GeolocCurrent(); 
    //Dictionary<int, ToDoItem> Items { get; set; } = new Dictionary<int, ToDoItem>();
    public Dictionary<DateTime, WeatherData> WeatherForcast { get; set; } = new Dictionary<DateTime, WeatherData>();

    public SessionService()
    {

    }
}
