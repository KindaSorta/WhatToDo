

namespace WhatToDo;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(ToDoItemDetailsPage), typeof(ToDoItemDetailsPage));
    }
}
