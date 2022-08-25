
namespace WhatToDo.View;

public partial class ToDoItemDetailsPage : ContentPage
{
	public ToDoItemDetailsPage(ToDoItemDetailsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}