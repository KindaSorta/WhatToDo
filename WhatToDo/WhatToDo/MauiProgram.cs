


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

                fonts.AddFont("Bauhaus93.ttf", "Bauhaus93");
            });


        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);

        builder.Services.AddSingleton<IDataService, DataService>();
        builder.Services.AddSingleton<IDialogService, DialogService>();
        builder.Services.AddSingleton<WeatherService>();

        builder.Services.AddSingleton<ISessionService, SessionService>();

        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<MainPage>();

        builder.Services.AddTransient<ToDoItemDetailsViewModel>();
        builder.Services.AddTransient<ToDoItemDetailsPage>();

        builder.Services.AddTransient<WeatherPreferencePopUpViewModel>();
        builder.Services.AddTransient<WeatherPreferencePopUpView>();
        builder.Services.AddTransient<BlankTemplate>();

        return builder.Build();
	}
}
