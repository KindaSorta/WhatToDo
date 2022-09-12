using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToDo.Model
{
    public static class ForecastDescrib
    {
        public static List<string> KeyWords { get; private set; } = new List<string>()
        {
            "None",
            "Sun",
            "Cloud",
            "Rain",
            "Thunderstorm",
            "Snow",
            "Blizzard"
        };
    }
}
