namespace WhatToDo.Views.Pages;

public partial class PomodoroTimerPage : ContentPage
{
	public PomodoroTimerPage(PomodoroTimerViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}