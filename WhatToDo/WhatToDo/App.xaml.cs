namespace WhatToDo;

public partial class App : Application
{
	public App(ShellViewModel viewModel)
	{
		InitializeComponent();

		MainPage = new AppShell(viewModel);
	}
}
