

namespace WhatToDo;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(ToDoItemDetailsPage), typeof(ToDoItemDetailsPage));
        Routing.RegisterRoute(nameof(PomodoroTimerPage), typeof(PomodoroTimerPage));
        Routing.RegisterRoute(nameof(SuggestionPage), typeof(SuggestionPage));
        Routing.RegisterRoute(nameof(CustomListPage), typeof(CustomListPage));
    }
}
