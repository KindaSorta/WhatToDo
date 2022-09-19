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
            "",
            "Recent",
            "Upcoming",
            "Priority",
            "Urgency",
            "Suggested"
        };

        public static readonly Dictionary<int, Color> PriorityColors = new Dictionary<int, Color>()
        {
            { 0, Colors.LightSkyBlue},
            { 1, Colors.Cyan},
            { 2, Colors.Aquamarine},
            { 3, Colors.Green},
            { 4, Colors.YellowGreen},
            { 5, Colors.Yellow},
            { 6, Colors.Gold},
            { 7, Colors.Orange},
            { 8, Colors.OrangeRed},
            { 9, Colors.Red},
            { 10, Colors.DarkRed}
        };
    }
}
