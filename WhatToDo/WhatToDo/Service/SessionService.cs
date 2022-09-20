

namespace WhatToDo.Service;

public class SessionService : ISessionService
{
    //public int Id { get; set; }

    public bool IsStarting { get; set; } = false;

    public GeolocCurrent CurrentGeolocInfo { get; set; } = new GeolocCurrent(); 
    //Dictionary<int, ToDoItem> Items { get; set; } = new Dictionary<int, ToDoItem>();
    public List<WeatherData> WeatherForcast { get; set; } = new List<WeatherData>();

    public ToDoItem ItemToEdit { get; set; } = new ToDoItem();
    public DateTime LastUpdated { get; set; }

    public SessionService()
    {

    }
}
