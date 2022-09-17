
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToDo.Views.Templates
{
    internal class CustomDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate WeatherPreferenceTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            switch (item)
            {
                case WeatherPreference w:
                    return WeatherPreferenceTemplate; ;
                default:
                    return default;
                case null:
                    throw new ArgumentNullException(nameof(item));
            }
        }
    }
}
