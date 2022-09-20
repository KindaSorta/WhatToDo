

//using AndroidX.Lifecycle;

namespace WhatToDo;

public partial class AppShell : Shell
{
    private ShellViewModel _viewModel;

	public AppShell(ShellViewModel viewModel)
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(ToDoItemDetailsPage), typeof(ToDoItemDetailsPage));
        Routing.RegisterRoute(nameof(PomodoroTimerPage), typeof(PomodoroTimerPage));
        Routing.RegisterRoute(nameof(SuggestionPage), typeof(SuggestionPage));
        Routing.RegisterRoute(nameof(CustomListPage), typeof(CustomListPage));
        Routing.RegisterRoute(nameof(WeatherPage), typeof(WeatherPage));
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Task.FromResult(_viewModel.InitialLoad());
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
}
