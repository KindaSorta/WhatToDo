using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToDo.Model
{
    public record ObjectsForDisplay
    {


    }

    public record DisplayWeatherPreference()
    {
        public WeatherPreference Preference { get; set; }


        public DisplayWeatherPreference(WeatherPreference preference) : this()
        {
            this.Preference = preference;
        }
    }
}
