
namespace WhatToDo.Model;

public class GeolocUpdateInterval
{
    public Location Location { get; set; }
    public TimeSpan Interval { get; set; }
    public DateTime LastUpdate { get; set; }    
}
