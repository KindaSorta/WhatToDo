

namespace WhatToDo.Model;

//public List<string> Forcast { Sunny, Rain, Thunderstorm, Cloudy, Snow, Hail, FreezingRain, Blizzard }
public enum Seasons { Spring, Summer, Fall, Winter }

public class WeatherType
{
    public Seasons Season { get; set; }
    public string Forcast { get; set; } = String.Empty;
    public string High { get; set; } = String.Empty;
    public string Low { get; set; } = String.Empty;
    public string WindSpeed { get; set; } = String.Empty;

    public WeatherType(string forcast, string high, string low, string wind, Seasons season)
    {
        Forcast = forcast;
        High = high;
        Low = low;
        WindSpeed = wind;
        Season = season;
    }

    public WeatherType(string forcast)
    {
        Forcast = forcast;
    }

    public WeatherType(string forcast, string high, string low)
    {
        Forcast = forcast;
        High = high;
        Low = low;
    }

    public WeatherType(string forcast, string high, string low, string wind)
    {
        Forcast = forcast;
        High = high;
        Low = low;
        WindSpeed = wind;
    }
}
