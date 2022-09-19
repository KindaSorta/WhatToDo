

using System.Reflection;

namespace WhatToDo.Model;

public enum Seasons { None, Spring, Summer, Fall, Winter }

public record WeatherPreference : IModelObject<WeatherPreference>
{
    public Seasons Season { get; set; } = Seasons.None;
    public string Forcast { get; set; } = String.Empty;
    public int High { get; set; }
    public int Low { get; set; }
    public int WindSpeed { get; set; }

    public WeatherPreference() { }

    public WeatherPreference(string forcast, int high, int low, int wind, Seasons season)
    {
        Forcast = forcast;
        High = high;
        Low = low;
        WindSpeed = wind;
        Season = season;
    }

    public WeatherPreference(string forcast)
    {
        Forcast = forcast;
    }

    public WeatherPreference(string forcast, int high, int low)
    {
        Forcast = forcast;
        High = high;
        Low = low;
    }

    public WeatherPreference(string forcast, int high, int low, int wind)
    {
        Forcast = forcast;
        High = high;
        Low = low;
        WindSpeed = wind;
    }

    public void CopyFrom(WeatherPreference item)
    {
        this.Forcast = item.Forcast;
        this.High = item.High;
        this.Low = item.Low;
        this.WindSpeed = item.WindSpeed;
        this.Season = item.Season;
    }

    public void CopyTo(WeatherPreference item)
    {
        item.Forcast = this.Forcast;
        item.High = this.High;
        item.Low = this.Low;
        item.WindSpeed = this.WindSpeed;
        item.Season = this.Season;
    }

    public bool IsValid()
    {
        if ( High < Low || 
            WindSpeed < 0 ||
            this.Equals(new WeatherPreference()))
        {
            return false;
        }

        return true;
    }



/*    public bool Equals(WeatherType other)
    {
        if (other != null && 
            (other.High == High || 
            other.Low == Low || 
            other.WindSpeed == WindSpeed || 
            other.Forcast == Forcast || 
            other.Season == Season))
        {
            return true;
        }
        return false;
    }*/
}
