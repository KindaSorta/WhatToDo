using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToDo.Data
{
    public static class FormOptions
    {
        public static readonly List<string> WeatherOptions = new List<string>()
        {
            "None",
            "Sun",
            "Cloud",
            "Rain",
            "Thunderstorm",
            "Snow",
            "Blizzard"
        };

        public static readonly List<string> FilterOptions = new List<string>()
        {
            "None",
            "Completed",
            "Incomplete",
            "Recent",
            "Upcoming",
            "Priority"
        };
    }
}
