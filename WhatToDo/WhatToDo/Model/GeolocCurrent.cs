
namespace WhatToDo.Model;

public class GeolocCurrent
{
    public Location Location { get; set; } = null;
    public TimeSpan Interval { get; set; } = TimeSpan.Zero;
    public DateTime LastUpdate { get; set; } = new DateTime();
    public GeolocationAccuracy Accuracy { get; set; } = GeolocationAccuracy.Medium;
    public TimeSpan TimeOut { get; set; } = TimeSpan.FromSeconds(30);
    public DateTime NextUpdateTime => LastUpdate + Interval;

    public GeolocCurrent()
    {

    }

    public GeolocCurrent(GeolocationAccuracy accuracy, DateTime lastUpdate, Location location, TimeSpan interval, TimeSpan timeOut)
    {
        Location = location;
        Interval = interval;
        Accuracy = accuracy;
        LastUpdate = lastUpdate;
        TimeOut = timeOut;
    }

}
