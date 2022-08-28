using WhatToDo.Service;
using WhatToDo.Data;
using WhatToDo.View;

namespace WhatToDo;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<WeatherService>();

		//builder.Services.AddSingleton<DataStorage>();
		builder.Services.AddSingleton<IDataService, DataService>();
        //builder.Services.AddSingleton<IGeolocation>();

        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<MainPage>();

        builder.Services.AddTransient<ToDoItemDetailsViewModel>();
        builder.Services.AddTransient<ToDoItemDetailsPage>();

        return builder.Build();
	}
}
